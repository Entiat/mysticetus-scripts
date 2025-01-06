// Observer Left Right Matching.cs
// ======================================
// MIT License
//
// Copyright 2025 Mysticetus, LLC
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
// =======================================
//
// This script looks for a (case insensitive) "L" or "R" in a bearing-to field named "Left or Right" 
// to convert that to the name of an observer sitting on that side of the aircraft, as 
// described in the LOGBOOK entry sheet, fields "Starboard Observer" and "Port Observer"
//
// Recommended Settings:
//      Repeat Interval: 30 seconds
//      Run Mode: Both (collection for PSOs, editing for PMs)
//

// Field and sheet names. Change these to match the appropriate fields in your protocol.
var detectionSheetName = "SIGHTINGS";
var bearingToFieldName = "Left or Right";
var observerFieldName = "OBSERVER";

var logbookSheetName = "LOGBOOK";
var leftObserverFieldName = "Port Observer";
var rightObserverFieldName = "Starboard Observer";

// Get the ids for each row in the detection and logbook sheets
var dataRowIds = Data.GetRowIds(detectionSheetName);
if (dataRowIds.Count == 0)
     return;

var logbookRowIds = Data.GetRowIds(logbookSheetName);
if (logbookRowIds.Count == 0)
    return;

// Get the left and right observer names
if (Data.GetData(logbookSheetName, logbookRowIds.Last(), leftObserverFieldName) is not string leftObserver ||
    string.IsNullOrWhiteSpace(leftObserver))
{
    return;
}
if (Data.GetData(logbookSheetName, logbookRowIds.Last(), rightObserverFieldName) is not string rightObserver ||
    string.IsNullOrWhiteSpace(leftObserver))
{
    return;
}

// Scan each row, grab the L or R in the bearing-to field, set the appropriate name into the Observer field
foreach (var rowId in dataRowIds)
{
    // Check to see if we've been told to stop processing (app is exiting, or user pressed Cancel)
    if (cancel.IsCancellationRequested)
        return;

    if (Data.GetData(detectionSheetName, rowId, bearingToFieldName) is not string bearingTo)
        continue;

    var observerName = bearingTo.ToLower() switch
    {
        "l" => leftObserver,
        "r" => rightObserver,
        _ => ""
    };
    if (string.IsNullOrWhiteSpace(observerName))
        continue;

    Data.SetData(detectionSheetName, rowId, observerFieldName, observerName);
}
