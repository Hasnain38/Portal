using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Portal.System_DataDefinition
{
    [Table("SystemDataDefinitions")]
    public class SystemDataDefinition : Entity
    {

        public virtual int DefTypeId { get; set; }

        public virtual string DefValue { get; set; }

        public virtual int DefParentId { get; set; }

        public virtual int EntityId { get; set; }

    }
}