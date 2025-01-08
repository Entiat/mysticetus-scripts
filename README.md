# mysticetus-scripts

This repo contains scripts for use in the Mysticetus app. Mysticetus introduced the ability to run user-created C# scripts in build 2022.32 (July 2022). A major update to scripting functionality was introduced in 2025.1 (January 2025) that brings APIs for scripts in line with APIs available for Mysticetus Plugins (1-1 compatibility).

All scripts hosted here live under the MIT license - you can use these for anything, anywhere, anytime, anyhow. All the disclaimers in the license apply, of course. These scripts are not guaranteed or warrantied to be useful for anything, at all, ever. Like all MIT-licensed software, they may have bugs, you may lose data, they may crash apps or crash your computer, they may cause heartbreak and you may need therapy, the therapist may sleep with your crush and your crush may hate you forever. That's all on you. Use these scripts at your own risk.

# Script Submission

Submit a PR with your script. It must contain the MIT License in the header as a C# comment. A template is provided below. By submitting your script you are agreeing to the license - anyone at all can use your script for anything, modify it, resell it, yell at it, write a song about it and earn a spot in the Rock and Roll Hall of Fame - whatever they want (as long as they preserve the MIT license with it).

Feel free to include additional comments about usage, gotchas, data schemas this works with, etc.

Please include the year and copyright holder in there. DO NOT put identifiable info (email, phone, etc.) in there. This is a public repo no doubt scanned by zillions of data harvester bots.

# MIT License Template
```csharp
    // MIT License
    //
    // Copyright <YEAR> <COPYRIGHT HOLDER>
    //
    // Permission is hereby granted, free of charge, to any person obtaining a copy of this
    // software and associated documentation files (the "Software"), to deal in the Software
    // without restriction, including without limitation the rights to use, copy, modify,
    // merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
    // permit persons to whom the Software is furnished to do so.
    // 
    // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
    // INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
    // PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
    // HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
    // OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
    // SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
    //
```
# Script Support
Currently, the following Mysticetus functions are available for scripts. These intentionally mirror APIs available to Mysticetus plugins (which are Windows assemblies intended to be more complex than scripts). We will be adding more functionality over time. Let us know what features you'd like to have access to or if we can improve the current API in any way.
```csharp
    #region Analysis
    /// <summary>
    /// Analysis interfaces
    /// </summary>
    public static class Analysis
    {
        /// <summary>
        /// Directs Mysticetus to run the specified analysis (as configured in Mysticetus settings) and 
        /// returns a list of binding source objects representing the results. Will throw exceptions on any error. Caller
        /// should catch and take appropriate action (pop UI, etc.)
        /// </summary>
        /// <param name="analysis">Name of the analysis to run.</param>
        /// <param name="vehicle">Name of the vehicle to use for the analysis, if the analysis is vehicle specific</param>
        /// <param name="combineAnalysis">If the analysis is an effort detail, tells Mysticetus whether or not to combine the analysis into a 
        /// single BindingSource, or return one BindingSource per effort bucket field.</param>
        /// <returns>List of binding sources that contain the results.</returns>
        /// <exception cref="InvalidOperationException">If the named Analysis does not exist.</exception>
        public static List<BindingSource> Run(string analysis, string vehicle, bool combineAnalysis = false);
    }
    #endregion

    #region Comms
    /// <summary>
    /// Communications mechanisms.
    /// </summary>
    public static class Comms
    {
        /// <summary>
        /// Gets whether or not the GPS currently has a fix
        /// </summary>
        public static bool GpsFixed { get; }

        /// <summary>
        /// Gets the time a gps (fixed) signal was last received. DateTime(0) if never.
        /// </summary>
        public static DateTime CommsGpsLastReceived { get; }

        /// <summary>
        /// Sends an email message immediately, with optional attachments (either embedded or inline), and ability to enqueue the email for later sending 
        /// if the initial attempt at send fails for any reason.
        /// </summary>
        /// <param name="from">Name of from entity eg "Jane Doe"</param>
        /// <param name="to">Dictionary of recipients - key is full name, value is email address</param>
        /// <param name="subject">Message subject</param>
        /// <param name="textBody">Message text body (may be empty string)</param>
        /// <param name="htmlBody">Message html body (may be empty string)</param>
        /// <param name="attachments">Any attachments. Keys are file names, values are contents. If embedded is true the attachment will be embedded, false means it is an actual attachment</param>
        /// <param name="enqueueOnFailure">Whether to enqueue for later sending if something goes wrong sending immediately.</param>
        /// <param name="progress">Progress updates</param>
        /// <param name="cancel">Cancellation Token</param>
        public static void SendMessage(
            string from,
            Dictionary<string, string> to,
            string subject,
            string textBody,
            string htmlBody,
            Dictionary<string, (bool embedded, byte[] attachment)> attachments,
            bool enqueueOnFailure,
            Action<double> progress,
            CancellationToken cancel);
    }
    #endregion

    #region Config
    /// <summary>
    /// Interactions with the configuration/settings in the Mysticetus app.
    /// </summary>
    public static class Config
    {
        public static void AddObserver(string name, double eyeHeight, string vehicle);

        /// <summary>
        /// Gets the current Mysticetus application run mode
        /// </summary>
        public static AppMode AppRunMode { get; }

        /// <summary>
        /// Retrieves the contents of the given named list, optionally for a specified vehicle.
        /// </summary>
        /// <param name="list">Named list to retrieve. Can include known lists such as {observers}, {obsplatforms}, etc.</param>
        /// <param name="vehicle">Optional vehicle to obtain list for. If null looks for "all vehicles"</param>
        /// <returns>List, optionally associated with this vehicle. Empty list on error. If requested list is special (eg {observers} the returned values are json with all attributes of the list items in each string.</returns>
        public static List<string> GetNamedList(string list, string? vehicle = null);

        /// <summary>
        /// Retrieves the list of all observers for the specified vehicle.
        /// </summary>
        /// <param name="vehicle">Name of vehicle to obtain observers for, if null looks for "all vehicles"</param>
        /// <returns>List of observers for the specified vehicle. Empty list on error.</returns>
        public static List<(string name, double eyeHeight)> GetObservers(string? vehicle = null);

        /// <summary>
        /// Retrieves a variable value, optionally for a given vehicle.
        /// </summary>
        /// <param name="variable">The variale value to retrieve</param>
        /// <param name="vehicle">If non-null, the variable associated with this vehicle. If null, looks for "all vehicles"</param>
        /// <returns>Variable value if found (optionally for specified vehicle) - null, otherwise </returns>
        public static string? GetVariableValue(string variable, string? vehicle = null);

        /// <summary>
        /// Gets the name (if any) of the file currently loaded into Mysticetus
        /// </summary>
        /// <returns>Name of the file, or "" if nothing loaded</returns>
        public static string LoadedFileName { get; }

        /// <summary>
        /// Parses a Path. Converts a number of variables, including %[DRIVELABEL]%, %MyDocuments%, %CommonDocuments, %TopoDir%, %OneDrive%, %DropBox%, %DropBoxBusiness%, %DropBoxPersonal%, %CitrixSharefile%,
        /// %Time% (hhmm), %Date% (yyyy-mm-dd), %StationId%.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="errors">[out]Parse errors, if any.</param>
        /// <returns>Parsed path, or string.Empty if savePath is null or empty</returns>
        public static string ParsePath(string path, List<string>? errors = null);

        /// <summary>
        /// Removes an observer from the system for a given vehicle.
        /// </summary>
        /// <param name="observer">Name of observer to remove</param>
        /// <param name="vehicle">if non-null, the vehicle to remove the observer from. Null removes from the "all vehicles" entry (not all vehicles)</param>
        public static void RemoveObserver(string observer, string? vehicle = null);

        /// <summary>
        /// Saves the map currently displayed in Mysticetus to the specified file.
        /// </summary>
        /// <param name="filename">Name of file to save map into. Supported extensions are ".jpg", ".jpeg", and ".png"</param>
        public static void SaveMap(string filename);
    }
    #endregion

    #region Data
    /// <summary>
    /// Interactions with data in the Mysticetus app.
    /// </summary>
    public static class Data
    {
        /// <summary>
        /// Add a row to the specified sheet
        /// </summary>
        /// <param name="sheet">Sheet to add a row to.</param>
        /// <param name="copyPrevRow">Whether or not to copy the previous row of data (if it exists) into the new row</param>
        /// <returns>Guid of new row, or Guid.Empty on error.</returns>
        public static Guid AddRow(string sheet, bool copyPrevRow);

        /// <summary>
        /// Determines whether or not a field in a sheet exists.
        /// </summary>
        /// <param name="sheet">Sheet to find.</param>
        /// <param name="field">Field in the sheet to find.</param>
        /// <returns>True if sheet exists and contains field named 'field'.</returns>
        public static bool FieldExists(string sheet, string field);

        /// <summary>
        /// Retrieves data from a field. 
        /// </summary>
        /// <param name="sheet">Sheet to access</param>
        /// <param name="rowId">Row number to access.</param>
        /// <param name="field">Field name to retrieve</param>
        /// <returns>Data, or null on error (or empty field). Times are returned as UTC DateTime.</returns>
        public static object? GetData(string sheet, Guid rowId, string field);

        /// <summary>
        /// Retrieves the list of row ids in the given sheet.
        /// </summary>
        /// <param name="sheet">Sheet name</param>
        /// <returns>RowIds of this sheet, or empty list on error.</returns>
        public static List<Guid> GetRowIds(string sheet);

        /// <summary>
        /// Gets the mean time of all loaded data. UTC.
        /// </summary>
        /// <returns>Mean time, or DateTime(0) on error or if nothing loaded</returns>
        public static DateTime MeanDateTime { get; }

        /// <summary>
        /// Sets a data value into a data field.
        /// </summary>
        /// <param name="sheet">Sheet to set a value into.</param>
        /// <param name="rowId">RowId to set the value into.</param>
        /// <param name="field">Field to set the value into.</param>
        /// <param name="data">Data to place in the row.</param>
        public static void SetData(string sheet, Guid rowId, string field, object? data);

        /// <summary>
        /// Gets whether or not a data sheet exists
        /// </summary>
        /// <param name="sheet">Sheet name</param>
        /// <returns>True if it exists, false if not.</returns>
        public static bool SheetExists(string sheet);

        /// <summary>
        /// Attempts to convert an object into a unix time (seconds since Jan 1, 1970). Will be negative if before 1970.
        /// Doubles are returned as-is. Ints are converted to double. Strings are parsed. Anything else returns double.NaN.
        /// </summary>
        /// <param name="time">Value to convert.</param>
        /// <returns>Seconds since Jan 1, 1970, or double.NaN if unable to parse/convert.</returns>
        public static double ToUnixTime(object? time);
    }
    #endregion

    #region Geo
    /// <summary>
    /// Interactions with Geospatial objects and functions in Mysticetus
    /// </summary>
    public static class Geo
    {
        /// <summary>
        /// Formats a latitude and longitude to match the current Location Format settings in Mysticetus.
        /// </summary>
        /// <param name="lat">WGS 84 latitude (negative south).</param>
        /// <param name="lon">WGS 84 longitude (negative west).</param>
        /// <returns>String formatted to match the current Location Format Settings in Mysticetus.</returns>
        public static string FormatLocation(double lat, double lon);

        /// <summary>
        /// Retreives a GeoJSON representation of a geospatial object. The 'properties' key contains additional Mysticetus-specific meta-data about the object.
        /// </summary>
        /// <param name="id">Id of the object. This is generally retrieved from Geo.GetAllObjects, or Data.GetRowIds (because for GeoSightings only, the last (in temporal order)
        /// data sheet row id corresponds to the GeoSighting guid)</param>
        /// <returns>GeoJSON representation of the geospatial object. The 'properties' key contains additional Mysticetus-specific meta-data. Return is "" if the id does not exist.</returns>
        public static string GetObject(Guid id);

        /// <summary>
        /// Retreives a list of information (id and type of object) about all the geo-spatial objects in the system.
        /// </summary>
        /// <param name="filter">Optional filter of types. Each entry in the array specifies a type, one of 
        /// [circle, polygon, sightingtrack, track, sighting, vehicle, gear, label, waypoint, plan]. 
        /// If this parameter is not specified or is empty all geo objects in the system will be returned.
        /// </param>
        /// <returns>List of information about all objects in the system</returns>
        public static List<(Guid id, string type)> GetAllObjects(params string[] filter);

        /// <summary>
        /// Gets or sets the primary vehicle's location.
        /// </summary>
        public static (double lat, double lon, double alt) PrimaryVehicleLocation { get; set; }

        /// <summary>
        /// Gets the Guid of the primary vehicle, or Guid.Empty if none found. Uses the following ordering:
        /// 1. Last vehicle used for data collection (if we're currently in data collection, use our primary vehicle)
        /// 2. If that's unknown (i.e. we're looking at a template), use the current primary vehicle (which might be null itself)
        /// </summary>
        public static Guid PrimaryVehicleGuid { get; };

        /// <summary>
        /// Gets the name of the primary vehicle, or empty if no primary vehicle.
        /// </summary>
        public static string PrimaryVehicleName { get; }
    }
    #endregion

    #region Report
    /// <summary>
    /// Functions and other support for generating reports
    /// </summary>
    public static class Report
    {
        /// <summary>
        /// Generates a report based on a general report format, optionally allowing the user to edit after generation, and optionally mailing the report to a list
        /// of users. The multitude of parameters controls how the report is formatted and where it obtains various data items. To create an even more custom 
        /// report, use the various ReportGenerate*Markdown functions directly and customize how they are organized in the report.
        /// </summary>
        /// <param name="options">Report options.</param>
        /// <param name="cancel">Cancellation token.</param>
        public static void GenerateAndSendReport(ReportOptions options, CancellationToken cancel);

        /// <summary>
        /// Generates effort markdown based on a report-specific analysis
        /// </summary>
        /// <param name="options">Report options</param>
        /// <param name="cancel">Cancellation token</param>
        /// <returns>Effort markdown to be included in the report.</returns>
        public static string GenerateEffortMarkdown(ReportOptions options, CancellationToken cancel);

        /// <summary>
        /// Generates effort timelines (changes) for the given EffortFields
        /// </summary>
        /// <param name="options">Reporting options</param>
        /// <param name="cancel">Cancellation token</param>
        /// <returns>Effort markdown to be included in the report.</returns>
        public static string GenerateEffortTimelines(ReportOptions options, CancellationToken cancel);

        /// <summary>
        /// Generates common markdown for a report based on ReportType. 
        /// </summary>
        /// <remarks>Uses a number of variables to fill in the blanks, namely:
        /// <code>
        ///		Project Name
        ///		Project Type
        ///		Survey Plan
        ///		PSO Provider
        ///		Survey Type
        ///		Survey Name
        ///		BOEM Lease (if available)
        ///		IHA (if available)
        /// </code>
        /// </remarks>
        /// <param name="reportType">Type of report we're generating.</param>
        /// <param name="workingDir">Working directory for storage of any associated images for the eventual markdown file.</param>
        /// <returns>Markdown representing a common format of header block for a report.</returns>
        public static string GenerateHeaderMarkdown(ReportType reportType, string workingDir);

        /// <summary>
        /// Generates markdown for including a map of the operation
        /// </summary>
        /// <param name="options">Reporting options</param>
        /// <param name="mapDirectory">Where to store the map file itself.</param>
        /// <returns>String representing generated map</returns>
        public static string GenerateMapMarkdown(ReportOptions options, string mapDirectory);

        /// <summary>
        /// Generates common markdown for the observation platforms in use.
        /// </summary>
        /// <param name="reportType">Type of report.</param>
        /// <returns>Markdown representing the observation platforms.</returns>
        public static string GenerateObservationPlatformsMarkdown(ReportType reportType);

        /// <summary>
        /// Generates common markdown for an observer summary from the Journal page.
        /// </summary>
        /// <param name="reportType">Type of report.</param>
        /// <returns>Markdown representing the observer summary.</returns>
        public static string GenerateObserverSummary(ReportType reportType);

        /// <summary>
        /// Generates common markdown for the list of observers for the primary collection vehicle.
        /// </summary>
        /// <param name="reportType">Type of report</param>
        /// <returns>Markdown representing list of observers for the primary collection vehicle.</returns>
        public static string GenerateObserversMarkdown(ReportType reportType);

        /// <summary>
        /// Generates common markdown for the list of recipients of this report
        /// </summary>
        /// <param name="reportType">Type of report</param>
        /// <returns>Markdown representing list of recipients for this report</returns>
        public static string GenerateRecipientsMarkdown(ReportType reportType);

        /// <summary>
        /// Generates markdown from the Description property of all the fields in the sheet. For example, if a field
        /// in the given data sheet has a boolean field with description "Did the trained lookout see any animals today" and that
        /// value is false, the output markdown for that field will be "Did the trained lookout see any animals today? No"
        /// </summary>
        /// <param name="reportType">Type of report</param>
        /// <param name="sheetName">Sheet to obtain data from (field descriptions and value/response)</param>
        /// <param name="sheetDesc">Description of sheet</param>
        /// <returns>Markdown representing Description/Value of each field.</returns>
        public static string GenerateSheetAsQuestionsMarkdown(ReportType reportType, string sheetName, string sheetDesc);

        /// <summary>
        /// Generates markdown of sheets as tables.
        /// </summary>
        /// <param name="options">Report options</param>
        /// <param name="name">Game of the sheet to generate a table from</param>
        /// <param name="fields">Information about which fields in the sheet to create a table</param>
        /// <returns></returns>
        public static string ReportGenerateSheetAsTableMarkdown(ReportOptions options, string name, string title, List<Field> fields);

        /// <summary>
        /// Generates markdown for sightings in the system, including recieved shared sightings.
        /// </summary>
        /// <param name="reportType">Type of report we're generating</param>
        /// <param name="sightingSheet">Sighting sheet</param>
        /// <param name="fields">List of sighting fields (field name and display name in report for each) to include with sighting details in the report. If the 
        /// field name contains ?? separating two field name, it is assumed the first field is boolean and the second field to the right of the ?? will be reported if the boolean is true. If the 
        /// boolean is false, nothing is reported. Example use of this is "Level B Exposure??Best Count" to report the number of exposures if the "Level B Exposure" checkbox is checked.</param>
        /// <returns>Sighting markdown for use in the report.</returns>
        public static string GenerateSightingsMarkdown(ReportType reportType, string sightingSheet, List<Field> fields);

        /// <summary>Generates vehicle speed markdown</summary>
        /// <param name="options">Options for this report.</param>
        /// <returns>Markdown representing a Vehicle Speed Summary</returns>
        public static string ReportGenerateVehicleSpeedMarkdown(ReportOptions options);

        /// <summary>
        /// Generates markdown representing vehicle speed summary from the GPS track log.
        /// </summary>
        /// <param name="options">Options for this report.</param>
        /// <returns>Markdown of speed summary from GPS log</returns>
        public static string GenerateVehicleSpeedFromGPSMarkdown(ReportOptions options);

        /// <summary>
        /// Generates markdown representing speed summary from observer records.
        /// </summary>
        /// <param name="options">Options for this report.</param>
        /// <returns>Markdown representing summary of speeds recorded by observer</returns>
        public static string GenerateVehicleSpeedFromSheetMarkdown(ReportOptions options);

        /// <summary>
        /// Generates weather markdown for the given report type.
        /// </summary>
        /// <param name="options">Options for the generation of the report markdown</param>
        /// <returns>Markdown representing a common format of weather information for a report.</returns>
        public static string GenerateWeatherMarkdown(ReportOptions options);

        /// <summary>
        /// Gets the name of a report type.
        /// </summary>
        /// <param name="reportType">The report type.</param>
        /// <returns>Name of the report type</returns>
        public static string GetReportTypeName(ReportType reportType);

        /// <summary>
        /// Prompts the user to select a report from the list of options.
        /// </summary>
        /// <returns>Report type selected by the user. ReportType.None if user presses cancel.</returns>
        public static ReportType SelectReportType(); 

        #region ReportType
        /// <summary>
        /// Types of reports this plugin supports.
        /// </summary>
        public enum ReportType
        {
            /// <summary>
            /// No report
            /// </summary>
            None = -1, // need none to be -1 because selection dialog starts with 0 and must match first valid report

            /// <summary>
            /// Avian daily report
            /// </summary>
            AvianDailyReport,

            /// <summary>
            /// Benthic Daily Report
            /// </summary>
            BenthicDailyReport,

            /// <summary>
            /// Crew daily report
            /// </summary>
            CrewDailyReport,

            /// <summary>
            /// HRG / GP Daily report
            /// </summary>
            GPDailyReport,

            /// <summary>
            /// GT Daily report
            /// </summary>
            GTDailyReport,

            /// <summary>
            /// PAM Operator report
            /// </summary>
            PAMReport,

            /// <summary>
            /// Pile driving daily report
            /// </summary>
            PileDriveDailyReport,

            /// <summary>
            /// Pile driving support daily report
            /// </summary>
            PileSupportDailyReport,
        }
        #endregion
    }
    #endregion

    #region UI
    /// <summary>
    /// Invokes UI interactions in the Mysticetus app
    /// </summary>
    public static class UI
    {
        /// <summary>
        /// Displays an input box with prompt to retrieve a line of user input
        /// </summary>
        /// <param name="title">Title of the input box</param>
        /// <param name="prompt">Prompt</param>
        /// <param name="initialText">Initial text to fill in the input box</param>
        /// <param name="allowEmptyText">Whether or not to allow empty text returned.</param>
        /// <returns>Text entered by the user</returns>
        public static string InputBox(string title, string prompt, string initialText, bool allowEmptyText);

        /// <summary>
        /// Shows a message box
        /// </summary>
        /// <param name="text">Text to display.</param>
        /// <param name="title">Title</param>
        /// <returns>Result of the form, or DialogResult.None on any error.</returns>
        public static DialogResult MessageBox(string text, string title, MessageBoxButtons buttons, MessageBoxIcon icon);

        /// <summary>
        /// Shows a form as a modal dialog.
        /// </summary>
        /// <param name="form">Form to display.</param>
        /// <returns>Result of the form, DialogResult.None on any error.</returns>
        public static DialogResult ShowDialog(Form form);

        /// <summary>
        /// Displays a fading popup window in the center of the screen.
        /// </summary>
        /// <param name="text">Text to display.</param>
        /// <param name="fadeTimeSeconds">Fade time in seconds</param>
        public static void ShowFadingPopup(string text, int fadeTimeSeconds, ScreenPosition initialPos = ScreenPosition.Center, ScreenPosition finalPos = ScreenPosition.Center);

        /// <summary>
        /// Displays a progress window, starts an action, and provides progress updating
        /// </summary>
        /// <param name="text">Text to display. The first time progress is updated, "...42%" (but correct number) will be appended and updated from there</param>
        /// <param name="action">Action to be run.</param>
        /// <returns>A progress handler to be updated by the task.</returns>
        public static void ShowProgress(string text, Action<IProgress<double>, CancellationToken> action);

        /// <summary>
        /// Shows a form with a list of items to choose.
        /// </summary>
        /// <param name="title">Title of the form.</param>
        /// <param name="contents">Contents of the list to choose from.</param>
        /// <param name="multiSelect">Whether or not this list supports multiple selection.</param>
        /// <param name="initialSelections">Which items are initially selected.</param>
        /// <returns>Indexes of the items selected, a single entry of -1 if cancel was pressed, empty array on error.</returns>
        public static int[] ShowSelectionDialog(string title, string[] contents, bool multiSelect, int[] initialSelections);

        /// <summary>
        /// Paints a standard button for SimpleUI (aka Crew), with optional image. Should be called from within a button's Paint event
        /// </summary>
        /// <param name="button">The button to custom draw.</param>
        /// <param name="e">The PaintEventArgs passed to the button's paint event</param>
        /// <param name="image">Optional image to draw on the button</param>
        public static void SimpleUIPaintButton(Button button, PaintEventArgs e, Image? image = null);
    }
    #endregion

    #region Units
    /// <summary>
    /// Units conversion functions (NOTE: currently only kph to kts...more will be added as they are needed)
    /// </summary>
    public static class Units
    {
        public static class Velocity
        {
            /// <summary>
            /// Converts kph to kts
            /// </summary>
            /// <param name="kph"></param>
            /// <returns></returns>
            public static double KphToKts(double kph);
        }
    }
    #endregion

    #region Utils
    /// <summary>
    /// General purpose utility functions
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Converts a bearing to an o'clock string.
        /// </summary>
        /// <param name="brg">The bearing to convert.</param>
        /// <returns>The o'clock representation of this bearing, or empty string on error.</returns>
        public static string ConvertBearingToOclock(double brg);

        /// <summary>
        /// Attempts to convert a string - in many formats - to a bearing value. Supports a wide range of standard bearings from the absolute
        /// bearing (e.g. 321.2), as well as constructs such as NNE, W, +30, 30R, 22L, 10:30.
        /// </summary>
        /// <param name="brg">The string bearing to convert.</param>
        /// <returns>The bearing, or double.NaN on conversion failure.</returns>
        public static double ConvertStringToBearing(string brg);

        /// <summary>
        /// Converts a docx file to an rtf file
        /// </summary>
        /// <param name="docxFile">Input docx file name</param>
        /// <param name="rtfFile">Output rtf file name</param>
        /// <param name="cancel">Cancellation token</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void ConvertDocxToRTF(string docxFile, string rtfFile, CancellationToken cancel);

        /// <summary>
        /// Converts a markdown file to a docx file. Installs pandoc from the internet if not already installed.
        /// </summary>
        /// <param name="markdownFile">Input markdown file to convert.</param>
        /// <param name="pandocReferenceFile">Reference doc(x) file passed to pandoc to define styles, headers, footers, etc. Should be created with
        /// 'pandoc -o pandoc-reference.docx --print-default-data-file reference.docx' and then further edited from there</param>
        /// <param name="docxFile">Output docx file</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void ConvertMarkdownToDocx(string markdownFile, string pandocReferenceFile, string docxFile, CancellationToken cancel);

        /// <summary>
        /// Formats a TimeSpan to look like "[3d] 17h 14m 12[.123]s".
        /// </summary>
        /// <param name="timeSpan">TimeSpan in question.</param>
        /// <param name="precision">Optional precision for seconds.</param>
        /// <returns>Formatted time span</returns>
        public static string FormatTimeSpan(TimeSpan timeSpan, int precision = 0);

        /// <summary>
        /// Determines the path to where pandoc is installed on this machine. If not found, returns ""
        /// </summary>
        /// <param name="installIfNotFound">If true, and pandoc cannot be found on the machine, attempts to download and install it.</param>
        public static string GetPandocPath(bool installIfNotFound);

        /// <summary>
        /// Retrieves a temporary directory to be used while Mysticetus is running.
        /// </summary>
        /// <returns>A temporary directory name</returns>
        public static string GetTempDir();

        /// <summary>
        /// Retrieves a uniquely named file in a temporary directory to be used while Mysticetus is running.
        /// </summary>
        /// <returns>A temporary file name.</returns>
        public static string GetTempFile();

        /// <summary>
        /// Tries to parse a date/time string into a DateTime object (UTC) and a timezone string. 
        /// </summary>
        /// <param name="value">Input value.</param>
        /// <param name="dateTimeUtc">Output date/time (utc) if parsed</param>
        /// <param name="timeZone">Output timezone if function succeeds and a timezone was found. Empty on error or if no timezone found.</param>
        /// <returns>True on success, false on error.</returns>
        public static bool TryParseDateTime(string value, out DateTime dateTimeUtc, out string timeZone);

        /// <summary>
        /// Tries to parse a location string into latitude, longitude and optional altitude.
        /// </summary>
        /// <param name="location">Location string.</param>
        /// <param name="loc">Output location values</param>
        /// <returns>Whether or not the parse was successful.</returns>
        public static bool TryParseLocation(string location, out (double lat, double lon, double alt) loc);

        /// <summary>
        /// TimeSpan.TryParse does not handle times with hours greater than 24 (it expects days as well). This function parses 
        /// strings with large hours properly
        /// </summary>
        /// <param name="value">String representation of a time span.</param>
        /// <param name="timeSpan">Output TimeSpan if parse was successful.</param>
        /// <returns>True on success, false on error.</returns>
        public static bool TryParseTimeSpan(string value, out TimeSpan timeSpan);
    }
    #endregion

    #region Watchdogs
    /// <summary>
    /// Interaction with Watchdogs in the Mysticetus app
    /// </summary>
    public static class Watchdogs
    {
        /// <summary>
        /// Removes the watchdog status for this named watchdog
        /// </summary>
        /// <param name="watchdogName">Name of watchdog</param>
        public static void Clear(string watchdogName);

        /// <summary>
        /// Sets the named watchdog status to OK.
        /// </summary>
        /// <param name="watchdogName"></param>
        /// <param name="watchdogDescription"></param>
        public static void SetOk(string watchdogName, string watchdogDescription);

        /// <summary>
        /// Sets Watchdog to a "Concern" state about data in an entry sheet.
        /// </summary>
        /// <param name="watchdogName">Name of watchdog</param>
        /// <param name="watchdogDescription">Description of this watchdog</param>
        /// <param name="error">Error text displayed to user when they click on Concern.</param>
        /// <param name="field">Field to bark at</param>
        /// <param name="rowId">Row to bark at</param>
        /// <param name="sheet">Sheet to bark at</param>
        public static void BarkAtSheetData(string watchdogName, string watchdogDescription, string error, string sheet, Guid rowId, string field);
    }
    #endregion
```
