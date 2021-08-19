/****** Object:  StoredProcedure [dbo].[spEmployeeUpdate]    Script Date: 8/19/2021 8:58:58 PM ******/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spEmployeeUpdate')
DROP PROCEDURE [dbo].[spEmployeeUpdate]
GO

/****** Object:  StoredProcedure [dbo].[spEmployeeUpdate]    Script Date: 8/19/2021 8:58:58 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Valora Mae Lagarnia
-- Create date: Aug. 17, 2021
-- Description:	Update Employee Details or Delete Employee
-- =============================================
CREATE PROCEDURE [dbo].[spEmployeeUpdate]
	@ID INT
	, @FullName varchar(100) = ''
	, @BirthDate Date = null
	, @TIN varchar(100) = ''
	, @EmployeeTypeId INT = 0
	, @IsDeleted BIT = 0
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF (@ISDeleted = 0)
	BEGIN
		UPDATE Employee 
		SET FullName = @FullName
			, Birthdate = @BirthDate
			, TIN = @TIN
			, EmployeeTypeId = @EmployeeTypeId
		WHERE ID = @ID
	END
	ELSE BEGIN
		UPDATE Employee 
		SET ISDeleted = @IsDeleted
		WHERE ID = @ID
	END
END
GO


