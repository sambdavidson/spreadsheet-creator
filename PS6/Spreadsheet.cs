// Samuel Davidson u0835059
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using SpreadsheetUtilities;
using System.Xml;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        /// <summary>
        /// Container for the active cells. 
        /// Since there are an infinite number of cells,
        /// it is useful to keep track of what cells are actually being worked with.
        /// </summary>
        private Dictionary<string, Cell> activeCells;
        /// <summary>
        /// Dependency graph for the cellDependencies.
        /// </summary>
        private DependencyGraph cellDependencies;
        /// <summary>
        /// The nameValidator that will be use anytime another cell is referenced to make sure that it is a valid cell.
        /// </summary>
        private Regex nameValidator;
        /// <summary>
        /// Data container for the public Changed property
        /// </summary>
        private bool _changed;
        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved                  
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed
        {
            get
            {
                return _changed;
            }
            protected set
            {
                _changed = value;
            }
        }

        /// <summary>
        /// Constructor for a Spreadsheet
        /// </summary>
        public Spreadsheet()
            : this(s => true, x => x, "default")
        {
        }
        /// <summary>
        /// Constructs a new spreadsheet. 
        /// Requires an isValid function that will be used to validate cell names.
        /// Requires a normalize function that is used to standardize all input cell data
        /// Requires a spreadsheet version name.
        /// </summary>
        /// <param name="isValid"></param>
        /// <param name="normalize"></param>
        /// <param name="version"></param>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version)
            : base(isValid,normalize,version)
        {
            activeCells = new Dictionary<string, Cell>();
            cellDependencies = new DependencyGraph();
            nameValidator = new Regex(@"^[a-zA-Z_]+\w*$");
        }
        /// <summary>
        /// Constructs a new spreadsheet. 
        /// Requires a filePath where the spreadsheet will be loaded.
        /// Requires an isValid function that will be used to validate cell names.
        /// Requires a normalize function that is used to standardize all input cell data
        /// Requires a spreadsheet version name.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="isValid"></param>
        /// <param name="normalize"></param>
        /// <param name="version"></param>
        public Spreadsheet(String filePath, Func<string, bool> isValid, Func<string, string> normalize, string version)
            : this(isValid, normalize, version)
        {
            try
            {
                using (XmlReader reader = XmlReader.Create(filePath))
                    //Using allows us to use the file on the HDD and then free it up for the OS afterwards
                {
                    if (reader.ReadToDescendant("spreadsheet") && reader.HasAttributes)
                    {
                        if (version != reader.GetAttribute("version")) //Ensure Version
                        {
                            throw new SpreadsheetReadWriteException("Version Mismatch");
                        }

                        while (reader.ReadToFollowing("cell"))
                        {
                            reader.ReadToDescendant("name"); //Read to the name
                            reader.Read(); //Advance to its value
                            String cellName = reader.Value; //Temporary container for name

                            reader.ReadToFollowing("contents"); //Read to the contents
                            reader.Read(); //Advance to its value
                            String cellContents = reader.Value; //Temporary container for value

                            try
                            {
                                SetContentsOfCell(cellName, cellContents); //Creat the cell using the read data
                            }
                            //Check for various exceptions that may occur and replace them with SpreadsheetReadWriteException
                            catch (Exception e)
                            {
                                throw new SpreadsheetReadWriteException(e.Message);
                            }


                        }
                    }
                    else
                        throw new SpreadsheetReadWriteException("Document header incorrect");
                }
            }
            catch (DirectoryNotFoundException)
            {
                throw new SpreadsheetReadWriteException("Directory not found");
            }
            catch (XmlException)
            {
                throw new SpreadsheetReadWriteException("Xml format error");
            }

        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<String> GetNamesOfAllNonemptyCells()
        {
            return new List<String>(activeCells.Keys);
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {
            name = Normalize(name);
            //Check if name isnt null
            if (!NameIsValid(name))
            {
                throw new InvalidNameException();
            }
            Cell outCell;
            if (activeCells.TryGetValue(name, out outCell))
            {
                if (outCell != null)
                    return outCell.Contents;
                else
                    return String.Empty;
            }

            return String.Empty;
                
        }
        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
        /// </summary>
        public override object GetCellValue(String name)
        {

            if (!NameIsValid(name))
            {
                throw new InvalidNameException();
            }
            Cell outCell;
            if (activeCells.TryGetValue(name, out outCell))
            {
                if (outCell != null)
                {
                    return outCell.Value;
                }
            }
            return String.Empty;
                
        }
        /// <summary>
        /// Helper function that for the GetCellValue method.
        /// Recursively find the values of all cells needed.
        /// </summary>
        private void CalculateCellValues(String name)
        {
            if (!NameIsValid(name))
            {
                throw new InvalidNameException();
            }
            Cell outCell;
            if (activeCells.TryGetValue(name, out outCell))
            {
                if (outCell != null)
                {
                    if (outCell.Contents.GetType() == typeof(Formula))
                    {
                        Formula formula = (Formula) outCell.Contents;
                        outCell.Value = formula.Evaluate(LookupCellValue);
                    }
                    else
                        outCell.Value = outCell.Contents;

                }
            }
            foreach (String str in cellDependencies.GetDependees(name)) //Recursively calculate for all dependees
            {
                CalculateCellValues(str);
            }
        }
        /// <summary>
        /// Private Lookup function for use with formula.Evaluate().
        /// Returns the double value of a cell or an ArgumentError if the cell does not contain a double.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private double LookupCellValue(string name)
        {
            if (!NameIsValid(name))
            {
                throw new ArgumentException();
            }
            Cell outCell;
            if (activeCells.TryGetValue(name, out outCell))
            {
                                    
                if (outCell.Value.GetType() == typeof(double))
                {
                    return (double) outCell.Value;
                }
            }
            throw new ArgumentException();
        }
 
        /// <summary>
        /// Returns the version information of the spreadsheet saved in the named file.
        /// If there are any problems opening, reading, or closing the file, the method
        /// should throw a SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        public override string GetSavedVersion(string filename)
        {
            try
            {
                using (XmlReader reader = XmlReader.Create(filename))
                    //Using allows us to use the file on the HDD and then free it up for the OS afterwards
                {
                    reader.ReadToDescendant("spreadsheet");
                    if (reader.HasAttributes)
                    {
                        return reader.GetAttribute("version");
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
                throw new SpreadsheetReadWriteException("Directory Not Found");
            }
            throw new SpreadsheetReadWriteException("XML Formatting");
        }

        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            var output = new HashSet<string>(cellDependencies.GetDependents(name));
            return output;
        }

        /// <summary>
        /// Helper class that recursively finds all dependees of name and returns a list of all dependees
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dependeeList"></param>
        /// <returns></returns>
        private void GetDependees(String name, ISet<String> dependeeList)
        {
            
            dependeeList.Add(name);          
            foreach (String str in cellDependencies.GetDependees(name))
            {
                GetDependees(str, dependeeList);
            }


        }

        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        /// 
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor.  There are then three possibilities:
        /// 
        ///   (1) If the remainder of content cannot be parsed into a Formula, a 
        ///       SpreadsheetUtilities.FormulaFormatException is thrown.
        ///       
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown.
        ///       
        ///   (3) Otherwise, the contents of the named cell becomes f.
        /// 
        /// Otherwise, the contents of the named cell becomes content.
        /// 
        /// If an exception is not thrown, the method returns a set consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            content = content.Trim();
            name = Normalize(name);

            if (content == null)
            {
                throw new ArgumentNullException();
            }

            Changed = true;

            ISet<String> outSet = null;
            double outBool;
            if (Double.TryParse(content, out outBool)) // Check if its a double
            {
                outSet  = SetCellContents(name, outBool);
            }
            else if (content.Length > 0 && content[0] == '=') //Check if its a formula
            {
                content = content.Remove(0, 1).Trim(); //Clean it up
                outSet = SetCellContents(name, new Formula(content, Normalize, IsValid));
            }
            else
                outSet = SetCellContents(name, content); // Otherwise just replace the value with whatever the string is

            CalculateCellValues(name);
            return outSet;
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<String> SetCellContents(String name, double number)
        {

            //Check if name isnt null and is valid
            if(!NameIsValid(name))
            {
                throw new InvalidNameException();
            }

            Cell outCell;
            //Check if the cell is active or if it needs to be created.
            if (!activeCells.TryGetValue(name, out outCell))
            {
                outCell = new Cell(name);
                activeCells.Add(name, outCell);
            }
            return SetCellContents(outCell, number);
        }
        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<String> SetCellContents(String name, String text)
        {
            //Check if text is null
            if (text == null)
            {
                throw new ArgumentNullException();
            }
            
            //Check if name isnt null and is valid
            if(!NameIsValid(name))
            {
                throw new InvalidNameException();
            }

            Cell outCell;
            //Check if the text is empty
            if (text.Length != 0)
            {
                //Check if the cell is active or if it needs to be created.
                if (!activeCells.TryGetValue(name, out outCell))
                {
                    outCell = new Cell(name);
                    activeCells.Add(name, outCell);
                }

                return SetCellContents(outCell, text);
            }
            ISet<String> tempDependees = new HashSet<String>(cellDependencies.GetDependees(name));
            tempDependees.Add(name);
            //Check if the cell is active and remove it if so.
            if (activeCells.TryGetValue(name, out outCell))
            {
                cellDependencies.ReplaceDependents(name, new HashSet<string>()); 
                activeCells.Remove(name);
            }
            return tempDependees;
        
        }
        /// <summary>
        /// If the formula parameter is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.  (No change is made to the spreadsheet.)
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<String> SetCellContents(String name, Formula formula)
        {
            //Checking for circular recursion using the method in AbstractSpreadsheet.
            if (formula == null)
            {
                throw new ArgumentNullException();
            }

            //Check if name isnt null and is valid
            if (!NameIsValid(name))
            {
                throw new InvalidNameException();
            }

            //Check for circular dependency
            IEnumerable<String> variables = formula.GetVariables();
            foreach (String var in variables)
            {
                if (name == var) //Obvious circular exception
                {
                    throw new CircularException();
                }
                CheckCircularDependency( name, var, new HashSet<String>() );
            }

            //Check if the cell exists
            Cell outCell;
            if (activeCells.TryGetValue(name, out outCell))
            {
                //Check if the cell already has a formula in it
                if (outCell.Contents.GetType() == typeof(Formula))
                {
                    //Remove old dependencies
                    Formula oldFormulas = (Formula) outCell.Contents;
                    IEnumerable<String> oldVariables = oldFormulas.GetVariables();
                    foreach (String var in oldVariables)
                    {
                        cellDependencies.RemoveDependency(name, var);
                    }
                        
                }

            }
            else
            {
                //If not we create a new cell and new dependencies
                outCell = new Cell(name);
                activeCells.Add(name, outCell);
            }

            //Add dependencies
            foreach(String var in variables)
            {
                cellDependencies.AddDependency(name, var);
            }
            outCell.Contents = formula;
            ISet<String> outSet = new HashSet<String>();
            GetDependees(name,outSet);

            return outSet;
        }

        /// <summary>
        /// This is our private SetCellContents method that does all of the "heavy lifting".
        /// This method will evaluate the content placed into the cells, update values, and dependencies.
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        private ISet<String> SetCellContents(Cell cell, object content)
        {
            //Check if the cell already has a formula in it
            if (cell.Contents != null)
            {
                if (cell.Contents.GetType() == typeof (Formula))
                {
                    //Remove old dependencies
                    Formula oldFormulas = (Formula) cell.Contents;
                    IEnumerable<String> oldVariables = oldFormulas.GetVariables();
                    foreach (String var in oldVariables)
                    {
                        cellDependencies.RemoveDependency(cell.Name, var);
                    }

                }
             }

        //Update contents
            cell.Contents = content;
            //Update dependencies
            cellDependencies.ReplaceDependents(cell.Name, new HashSet<string>());
            //Preparing output
            ISet<String> outSet = new HashSet<String>();
            GetDependees(cell.Name, outSet);

            return outSet;
        }

        /// <summary>
        /// Writes the contents of this spreadsheet to the named file using an XML format.
        /// The XML elements should be structured as follows:
        /// 
        /// <spreadsheet version="version information goes here">
        /// 
        /// <cell>
        /// <name>
        /// cell name goes here
        /// </name>
        /// <contents>
        /// cell contents goes here
        /// </contents>    
        /// </cell>
        /// 
        /// </spreadsheet>
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.  
        /// If the cell contains a string, it should be written as the contents.  
        /// If the cell contains a double d, d.ToString() should be written as the contents.  
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        /// 
        /// If there are any problems opening, writing, or closing the file, the method should throw a
        /// SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        public override void Save(string filename)
        {
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                using (XmlWriter writer = XmlWriter.Create(filename,settings))
                //Using allows us to use the file on the HDD and then free it up for the OS afterwards
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("version", Version);

                    foreach (Cell cell in activeCells.Values) //Save each cell
                    {
                        writer.WriteStartElement("cell");
                        writer.WriteElementString("name", cell.Name);
                        if (cell.Contents.GetType() == typeof(Formula))
                        {
                            writer.WriteElementString("contents", cell.Contents.ToString());
                        }
                        else
                        {
                            writer.WriteElementString("contents", cell.Contents.ToString());
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
            catch (DirectoryNotFoundException)
            {
                throw new SpreadsheetReadWriteException("Invalid Directory");
            }

        }

        /// <summary>
        /// Private helper method used to check for circular dependency.
        /// Functions similarly to the 'visit' method from the AbstractSpreadsheet.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="name"></param>
        /// <param name="visited"></param>
        private void CheckCircularDependency(String start, String name, ISet<String> visited)
        {
            visited.Add(name);
            foreach (String n in GetDirectDependents(name))
            {
                if (n.Equals(start)) //Check if the next place to visit is the beginning, therefor it is circular.
                {
                    throw new CircularException();
                }
                if (!visited.Contains(n))
                {
                    //Recursion
                    CheckCircularDependency(start, n, visited);
                }
            }
        }

        /// <summary>
        /// Helper method used for validating a name. Simplifies the SetCellContents methods.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool NameIsValid(String name)
        {
            return (name != null && nameValidator.IsMatch(name) && IsValid(name));
        }

        /// <summary>
        /// Private Cell class used for storing cell data. Stores the cell's name and contents.
        /// </summary>
        private class Cell
        {

            private readonly String _cellName;
            /// <summary>
            /// Cell constructor. Requires the name associated with it.
            /// </summary>
            /// <param name="cellName"></param>
            public Cell(String cellName)
            {
                _cellName = cellName;
            }
            /// <summary>
            /// The String name associated with the cell. Useful when finding dependencies
            /// </summary>
            public String Name { get { return _cellName; } }
            /// <summary>
            /// The contents of the cell, can be any object
            /// </summary>
            public object Contents { get; set; }
            /// <summary>
            /// The numerical value of the cell or its string 
            /// Can contain either a double, string, or formula error
            /// </summary>
            public object Value { get; set; }
            /// <summary>
            /// Overridden toString(). Not explicitely used but I thought would be useful to put in.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return _cellName;
            }

        }
    }
}
