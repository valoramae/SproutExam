/****** Object:  StoredProcedure [dbo].[spEmployeeInsert]    Script Date: 8/19/2021 8:58:49 PM ******/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spEmployeeInsert')
DROP PROCEDURE [dbo].[spEmployeeInsert]
GO

/****** Object:  StoredProcedure [dbo].[spEmployeeInsert]    Script Date: 8/19/2021 8:58:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Valora Mae Lagarnia
-- Create date: Aug. 17, 2021
-- Description:	Insert new Employee
-- =============================================
CREATE PROCEDURE [dbo].[spEmployeeInsert]
	@FullName varchar(100)
	, @BirthDate Date 
	, @TIN varchar(100)
	, @EmployeeTypeId INT
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	INSERT INTO Employee (FullName,BirthDate,TIN,EmployeeTypeId)
	VALUES (@FullName,@BirthDate,@TIN,@EmployeeTypeId)

	SELECT SCOPE_IDENTITY() 

END
GO


