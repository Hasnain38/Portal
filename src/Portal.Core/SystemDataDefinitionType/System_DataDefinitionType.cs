using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Portal.SystemDataDefinitionType
{
    [Table("System_DataDefinitionTypes")]
    public class System_DataDefinitionType : Entity
    {

        public virtual string DefTypeValue { get; set; }

        public virtual string DefTypeCode { get; set; }

        public virtual int DefTypeParentId { get; set; }

    }
}