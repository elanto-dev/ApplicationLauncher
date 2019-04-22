using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class StudyInformation
    {
        public string StudyId { get; set; }

        [Required]
        public string Patientssn { get; set; }

        [Required]
        public string PatientBirthdate { get; set; }

        [Required]
        public string PatientFirstName { get; set; }

        [Required]
        public string PatientLastName { get; set; }

        public string RefSenderDoctor { get; set; }

        public string StudyNumber { get; set; }

        public string SampleNumber { get; set; }

        [Required]
        public string StudyDate { get; set; }

        public string StudyInstanceUid { get; set; }

        public string StudyPerformer { get; set; }

        public string StudyTypeCode { get; set; }

        public string StudyTypeName { get; set; }

        public int ContainerCount { get; set; }

        public List<Container> Containers { get; set; }

        private string GetStringOfContainers()
        {
            if (Containers == null)
            {
                return "";
            }
            var resultSb = new StringBuilder();
            foreach (var container in Containers)
            {
                resultSb.Append(" -container");
                resultSb.Append(container.ContainerId);
                resultSb.Append(" ");
                resultSb.Append(container.ContainerValue);
            }
            return resultSb.ToString();
        }

        public string ToParametersString => "-patientssn " + Patientssn
                                                            + " -patientfirstname " + PatientFirstName
                                                            + " -patientlastname " + PatientLastName
                                                            + " -patientbirthdate " + PatientBirthdate
                                                            + " -studydate " + StudyDate
                                                            + (StudyNumber != null ? " -studynumber " + StudyNumber : "")
                                                            + (StudyId != null ? " -studyid " + StudyId : "")
                                                            + (SampleNumber != null ? " -samplenumber " + SampleNumber : "")
                                                            + (RefSenderDoctor != null ? " -refsenderdoc " + RefSenderDoctor : "")
                                                            + (StudyPerformer != null ? " -studyperformer " + StudyPerformer : "")
                                                            + (StudyTypeCode != null ? " -studytypecode " + StudyTypeCode : "")
                                                            + (StudyInstanceUid != null ? " -studyinstanceuid " + StudyInstanceUid : "")
                                                            + (StudyTypeName != null ? " -studytypename " + StudyTypeName : "")
                                                            + " -containercount " + ContainerCount + GetStringOfContainers();
    }
}
