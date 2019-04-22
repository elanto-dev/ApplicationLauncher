using System;
using System.Collections.Generic;
using System.Globalization;
using Domain;
using IniFileParser.Model;
using Container = Domain.Container;

namespace FileTransformerService
{
    public class FileTransformer
    {
        protected readonly string FullFilePath;

        public FileTransformer(string fullFilePath)
        {
            FullFilePath = fullFilePath;
        }

        public StudyInformation FileReader()
        {
            var parser = new IniFileParser.IniFileParser();
            var data = parser.ReadFile(FullFilePath);
            if (!SectionsExist(data))
            {
                return null;
            }
            
            var studyParameters = data["PARAMETERS"];
            var bottlesInformation = data["BOTTLES"];
            if (!AllNeededParametersExist(studyParameters))
            {
                return null;
            }
           

            SetValuesToStudyInfoParameters(studyParameters, out var studyInfo);

            if (BottlesExist(bottlesInformation, out var bottleCount))
            {
                SetValuesToAllBottles(bottlesInformation, bottleCount, out var containers);
                studyInfo.Containers = containers;
            }
            
            studyInfo.ContainerCount = bottleCount;
            return studyInfo;
        }

        private static bool SectionsExist(IniData data)
        {
            return data["PARAMETERS"] != null && data["BOTTLES"] != null;
        }

        private static bool BottlesExist(KeyDataCollection bottles, out int bottleCount)
        {
            bottleCount = 0;
            if (bottles["COUNT"] == null) return false;
            if (!int.TryParse(bottles["COUNT"], out var parseResult)) return false;
            bottleCount = parseResult;
            return bottleCount != 0;
        }

        private static bool AllNeededParametersExist(KeyDataCollection studyParameters)
        {
            return studyParameters["DOB"] != null
                   && studyParameters["PATIENTID"] != null
                   && studyParameters["PATIENTNAME"] != null
                   && studyParameters["STUDYDATE"] != null;
        }

        private static bool ParametersContainKey(KeyDataCollection parameters, string key, out string paramOut)
        {
            if (parameters.ContainsKey(key))
            {
                paramOut = parameters[key];
                return true;
            }

            paramOut = null;
            return false;
        }

        private static void SetValuesToStudyInfoParameters(KeyDataCollection studyParameters, out StudyInformation studyInformation)
        {
            studyInformation = new StudyInformation()
            {
                PatientBirthdate = DateTime.ParseExact(studyParameters["DOB"], "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd.MM.yyyy"),
                Patientssn = studyParameters["PATIENTID"],
                PatientFirstName = studyParameters["PATIENTNAME"].Substring(studyParameters["PATIENTNAME"].IndexOf(",", StringComparison.Ordinal) + 1),
                PatientLastName = studyParameters["PATIENTNAME"].Substring(0, studyParameters["PATIENTNAME"].IndexOf(",", StringComparison.Ordinal)),
                StudyDate = DateTime.ParseExact(studyParameters["STUDYDATE"], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).ToString("dd.MM.yyyy HH:mm"),
                ContainerCount = 0
            };
            if (ParametersContainKey(studyParameters, "SAMPLENUMBER", out var studyInfoParameter))
            {
                studyInformation.SampleNumber = studyInfoParameter;
            }

            if(ParametersContainKey(studyParameters, "ACCESSIONNUMBER", out studyInfoParameter))
            {
                studyInformation.StudyNumber = studyInfoParameter;
            }
            if (ParametersContainKey(studyParameters, "REFERRINGPHYSICIANSNAME", out studyInfoParameter))
            {
                studyInformation.RefSenderDoctor = studyInfoParameter;
            }
            if (ParametersContainKey(studyParameters, "STUDYID", out studyInfoParameter))
            {
                studyInformation.StudyId = studyInfoParameter;
            }
            if (ParametersContainKey(studyParameters, "STUDYPERFORMER", out studyInfoParameter))
            {
                studyInformation.StudyPerformer = studyInfoParameter;
            }
            if (ParametersContainKey(studyParameters, "STUDYTYPE", out studyInfoParameter))
            {
                studyInformation.StudyTypeName = studyInfoParameter;
            }
            if (ParametersContainKey(studyParameters, "STUDYCODE", out studyInfoParameter))
            {
                studyInformation.StudyTypeCode = studyInfoParameter;
            }
            if (ParametersContainKey(studyParameters, "STUDYINSTANCEUID", out studyInfoParameter))
            {
                studyInformation.StudyInstanceUid = studyInfoParameter;
            }
        }

        private static void SetValuesToAllBottles(KeyDataCollection bottlesInformation, int containerCounter, out List<Container> containerList)
        {
            containerList = new List<Container>();
            for (var counter = 1; counter <= containerCounter; counter++)
            {
                var iniKey = "BOTTLE" + counter;
                var newContainer = new Container()
                {
                    ContainerId = counter,
                    ContainerValue = bottlesInformation[iniKey]
                };
                containerList.Add(newContainer);
            }
        }
    }
}
