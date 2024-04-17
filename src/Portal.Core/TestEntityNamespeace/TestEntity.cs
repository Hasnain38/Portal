using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Portal.TestEntityNamespeace
{
    [Table("TestEntities")]
    public class TestEntity : Entity
    {

        public virtual string TestName { get; set; }

    }
}