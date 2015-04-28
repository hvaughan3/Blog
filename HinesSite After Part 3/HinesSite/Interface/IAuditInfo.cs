#region Usings

using System;

#endregion
// ReSharper disable MissingXmlDoc

namespace HinesSite.Interface {

    public interface IAuditInfo {

        DateTime CreatedOn { get; set; }

        // Made ModifiedOn optional since it must be explicitly don't for DateTime types
        // Also had to add the optional parameter here and in the models using ModifiedOn
        DateTime? ModifiedOn { get; set; }
    }
}
