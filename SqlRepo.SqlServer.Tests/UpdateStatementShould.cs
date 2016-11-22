﻿using System;
using System.Linq.Expressions;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Testing;

namespace SqlRepo.SqlServer.Tests
{
    [TestFixture]
    public class UpdateStatementShould : SqlCommandTestBase<UpdateCommand<TestEntity>, int>
    {
        [Test]
        public void DefaultSchemaToDbo()
        {
            this.Command.TableSchema.Should()
                .Be("dbo");
        }

        [Test]
        public void DefaultTableNameToNameOfType()
        {
            this.Command.TableName.Should()
                .Be("TestEntity");
        }

        [Test]
        public void SupportUsingSpecificSchema()
        {
            this.Command.UsingTableSchema(OtherValue);
            this.Command.TableSchema.Should()
                .Be(OtherValue);
        }

        [Test]
        public void SupportChainingAfterSettingTableSchema()
        {
            this.Command.UsingTableSchema(OtherValue)
                .Should()
                .Be(this.Command);
        }

        [Test]
        public void SupportUsingSpecificTableName()
        {
            this.Command.UsingTableName(OtherValue);
            this.Command.TableName.Should()
                .Be(OtherValue);
        }

        [Test]
        public void SupportChainingAfterSettingTableName()
        {
            this.Command.UsingTableName(OtherValue)
                .Should()
                .Be(this.Command);
        }

        [Test]
        public void ProduceCorrectDefaultTableSpecfication()
        {
            this.Command.Set(e => e.IntProperty, 1)
                .Sql()
                .Should()
                .Contain(this.ExpectedTableSpecification(this.Command.TableSchema, this.Command.TableName));
        }

        [Test]
        public void ProduceCorrectNonDefaultTableSpecfication()
        {
            this.Command.Set(e => e.IntProperty, 1)
                .UsingTableSchema(OtherValue)
                .UsingTableName(OtherValue)
                .Sql()
                .Should()
                .Contain(this.ExpectedTableSpecification(OtherValue, OtherValue));
        }

        [Test]
        public void BeCleanByDefault()
        {
            this.Command.IsClean.Should()
                .BeTrue();
        }

        [Test]
        public void BuildCorrectSetClauseWithOnlyStringPropertySet()
        {
            const string expected = "UPDATE [dbo].[TestEntity]\nSET [StringProperty] = 'My Name'";
            this.Command.Set(e => e.StringProperty, "My Name")
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void BuildCorrectSetClauseWithOnlyDateTimePropertySet()
        {
            const string expected = "UPDATE [dbo].[TestEntity]\nSET [DateTimeProperty] = '{0}'";
            var now = DateTime.UtcNow;
            this.Command.Set(e => e.DateTimeProperty, now)
                .Sql()
                .Should()
                .StartWith(string.Format(expected, now.ToString(FormatString.DateTime)));
        }

        [Test]
        public void BuildCorrectSetClauseWithOnlyNullableDateTimePropertySet()
        {
            const string expected = "UPDATE [dbo].[TestEntity]\nSET [NullableDateTimeProperty] = '{0}'";
            var now = DateTime.UtcNow;
            this.Command.Set(e => e.NullableDateTimeProperty, now)
                .Sql()
                .Should()
                .StartWith(string.Format(expected, now.ToString(FormatString.DateTime)));
        }

        [Test]
        public void BuildCorrectSetClauseWithOnlyDateTimeOffsetPropertySet()
        {
            const string expected = "UPDATE [dbo].[TestEntity]\nSET [DateTimeOffsetProperty] = '{0}'";
            var now = DateTimeOffset.UtcNow;
            this.Command.Set(e => e.DateTimeOffsetProperty, now)
                .Sql()
                .Should()
                .StartWith(string.Format(expected, now.ToString(FormatString.DateTimeOffset)));
        }

        [Test]
        public void BuildCorrectSetClauseWithOnlyNullableDateTimeOffsetPropertySet()
        {
            const string expected = "UPDATE [dbo].[TestEntity]\nSET [NullableDateTimeOffsetProperty] = '{0}'";
            var now = DateTimeOffset.UtcNow;
            this.Command.Set(e => e.NullableDateTimeOffsetProperty, now)
                .Sql()
                .Should()
                .StartWith(string.Format(expected, now.ToString(FormatString.DateTimeOffset)));
        }

        [Test]
        public void BuildCorrectSetClauseWithOnlyIntPropertySet()
        {
            const string expected = "UPDATE [dbo].[TestEntity]\nSET [IntProperty] = 1";
            this.Command.Set(e => e.IntProperty, 1)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void BuildCorrectSetClauseWithOnlyDoublePropertySet()
        {
            const string expected = "UPDATE [dbo].[TestEntity]\nSET [DoubleProperty] = 1.01";
            this.Command.Set(e => e.DoubleProperty, 1.01)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void BuildCorrectSetClauseWithOnlyDecimalPropertySet()
        {
            const string expected = "UPDATE [dbo].[TestEntity]\nSET [DecimalProperty] = 2.01";
            this.Command.Set(e => e.DecimalProperty, 2.01M)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void BuildCorrectSetClauseWithOnlySinglePropertySet()
        {
            const string expected = "UPDATE [dbo].[TestEntity]\nSET [SingleProperty] = 1.01";
            this.Command.Set(e => e.SingleProperty, 1.01)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void BuildCorrectSetClauseWithOnlyShortPropertySet()
        {
            const string expected = "UPDATE [dbo].[TestEntity]\nSET [ShortProperty] = 1";
            this.Command.Set(e => e.ShortProperty, 1)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void BuildCorrectSetClauseWithOnlyBytePropertySet()
        {
            const string expected = "UPDATE [dbo].[TestEntity]\nSET [ByteProperty] = 1";
            this.Command.Set(e => e.ByteProperty, 1)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void BuildCorrectSetClauseWithOnlyGuidPropertySet()
        {
            const string expected = "UPDATE [dbo].[TestEntity]\nSET [GuidProperty] = '{0}'";
            var guid = Guid.NewGuid();
            this.Command.Set(e => e.GuidProperty, guid)
                .Sql()
                .Should()
                .StartWith(string.Format(expected, guid));
        }

        [Test]
        public void BuildCorrectSetClauseWithOnlyTestEnumPropertySet()
        {
            const string expected = "UPDATE [dbo].[TestEntity]\nSET [TestEnumProperty] = 1";
            this.Command.Set(e => e.TestEnumProperty, TestEnum.One)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void BuildCorrectSetClauseWithMultipleSets()
        {
            const string expected =
                "UPDATE [dbo].[TestEntity]\nSET [IntProperty] = 1, [StringProperty] = 'My String'";
            this.Command.Set(e => e.IntProperty, 1)
                .Set(e => e.StringProperty, "My String")
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void UserBuilderOnWhere()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 5;
            this.Command.Set(e => e.IntProperty, 1)
                .Where(expression);
            this.WhereClauseBuilder.Received()
                .Where(expression);
        }

        [Test]
        public void UserWhereBuilderOnAnd()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 5;
            this.Command.Set(e => e.IntProperty, 1)
                .Where(expression)
                .And(expression);
            this.WhereClauseBuilder.Received()
                .And(expression);
        }

        [Test]
        public void UserWhereBuilderOnOr()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 5;
            this.Command.Set(e => e.IntProperty, 1)
                .Where(expression)
                .Or(expression);
            this.WhereClauseBuilder.Received()
                .Or(expression);
        }

        [Test]
        public void UserWhereBuilderOnNestedAnd()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 5;
            this.Command.Set(e => e.IntProperty, 1)
                .Where(expression)
                .NestedAnd(expression);
            this.WhereClauseBuilder.Received()
                .NestedAnd(expression);
        }

        [Test]
        public void UserWhereBuilderOnNestedOr()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 5;
            this.Command.Set(e => e.IntProperty, 1)
                .Where(expression)
                .NestedOr(expression);
            this.WhereClauseBuilder.Received()
                .NestedOr(expression);
        }

        [Test]
        public void UseBuilderOnBuild()
        {
            this.Command.Set(e => e.IntProperty, 1)
                .Set(e => e.StringProperty, "My String")
                .Where(e => e.Id == 55)
                .Sql();
            this.WhereClauseBuilder.Received()
                .Sql();
        }

        [Test]
        public void ResetWhereClauseOnFromScratch()
        {
            this.Command.FromScratch();
            this.WhereClauseBuilder.Received()
                .FromScratch();
        }

        [Test]
        public void EmbedWhereClauseFromBuilderInStatement()
        {
            const string whereClause = "WHERE [Id] = 55";
            const string expected =
                "UPDATE [dbo].[TestEntity]\nSET [IntProperty] = 1, [StringProperty] = 'My String'\n"
                + whereClause + ";";
            this.WhereClauseBuilder.Sql()
                .Returns(whereClause);
            var result = this.Command.Set(e => e.IntProperty, 1)
                             .Set(e => e.StringProperty, "My String")
                             .Where(e => e.Id == 55)
                             .Sql();
            result.Should()
                  .Be(expected);
        }

        [Test]
        public void BuildCorrectStatementFromEntity()
        {
            this.AssumeWhereClauseBuilderReportsClean();
            this.AssumeTestEntityIsInitialised();
            var expected =
                $"UPDATE [dbo].[TestEntity]\nSET [DateTimeOffsetProperty] = '{this.Entity.DateTimeOffsetProperty.ToString(FormatString.DateTimeOffset)}', [NullableDateTimeOffsetProperty] = '{this.Entity.NullableDateTimeOffsetProperty.Value.ToString(FormatString.DateTimeOffset)}', [DateTimeProperty] = '{this.Entity.DateTimeProperty.ToString(FormatString.DateTime)}', [NullableDateTimeProperty] = '{this.Entity.NullableDateTimeProperty.Value.ToString(FormatString.DateTime)}', [DoubleProperty] = {this.Entity.DoubleProperty}, [IntProperty] = {this.Entity.IntProperty}, [IntProperty2] = {this.Entity.IntProperty2}, [StringProperty] = '{this.Entity.StringProperty}', [TestEnumProperty] = {(int)this.Entity.TestEnumProperty}, [DecimalProperty] = {this.Entity.DecimalProperty}, [ByteProperty] = {this.Entity.ByteProperty}, [ShortProperty] = {this.Entity.ShortProperty}, [SingleProperty] = {this.Entity.SingleProperty}, [GuidProperty] = '{this.Entity.GuidProperty}'\nWHERE [Id] = {this.Entity.Id};";
            this.Command.For(this.Entity)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ThrowExceptionIfBuildCalledWithoutInitialisingStatement()
        {
            this.Command.Invoking(s => s.Sql())
                .ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void ThrowExceptionIfForCalledAfterWith()
        {
            this.AssumeTestEntityIsInitialised();
            this.AssumeWhereClauseBuilderReportsClean();
            this.Command.FromScratch()
                .For(this.Entity);
            this.Command.Invoking(s => s.Set(e => e.ByteProperty, 1))
                .ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void ThrowExceptionIfWithCalledAfterFor()
        {
            this.AssumeTestEntityIsInitialised();
            this.Command.Set(e => e.ByteProperty, 1);
            this.Command.Invoking(s => s.For(this.Entity))
                .ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void ThrowExceptionIfForCalledAfterWhere()
        {
            this.AssumeTestEntityIsInitialised();
            this.Command.Where(e => e.ByteProperty == 1);
            this.Command.Invoking(s => s.For(this.Entity))
                .ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void ThrowExceptionIfWhereCalledAfterFor()
        {
            this.AssumeWhereClauseBuilderReportsClean();
            this.AssumeTestEntityIsInitialised();
            this.Command.For(this.Entity);
            this.Command.Invoking(s => s.Where(e => e.ByteProperty == 1))
                .ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void ExecuteQueryOnGo()
        {
            const string expected = "UPDATE [dbo].[TestEntity]\nSET [StringProperty] = 'My Name';";
            this.AssumeGoIsRequested();
            this.CommandExecutor.Received()
                .ExecuteNonQuery(expected);
        }

        protected override UpdateCommand<TestEntity> CreateCommand(ICommandExecutor commandExecutor,
            IEntityMapper entityMapper,
            IWritablePropertyMatcher writablePropertyMatcher,
            ISelectClauseBuilder selectClauseBuilder,
            IFromClauseBuilder fromClauseBuilder,
            IWhereClauseBuilder whereClauseBuilder,
            string connectionString)
        {
            var command = new UpdateCommand<TestEntity>(commandExecutor,
                entityMapper,
                writablePropertyMatcher,
                whereClauseBuilder);
            command.UseConnectionString(connectionString);
            return command;
        }

        private void AssumeGoIsRequested()
        {
            this.Command.Set(e => e.StringProperty, "My Name")
                .Go();
        }

        private string ExpectedTableSpecification(string schema, string table)
        {
            return $"UPDATE [{schema}].[{table}]\nSET";
        }
    }
}