/****** Object:  StoredProcedure [dbo].[spEmployeeGet]    Script Date: 8/19/2021 8:56:12 PM ******/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spEmployeeGet')
DROP PROCEDURE [dbo].[spEmployeeGet]
GO

/****** Object:  StoredProcedure [dbo].[spEmployeeGet]    Script Date: 8/19/2021 8:56:12 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Valora Mae Lagarnia
-- Create date: Aug. 17, 2021
-- Description:	Select Employee List
-- =============================================
CREATE PROCEDURE [dbo].[spEmployeeGet]
	@ID int = 0
	, @TIN varchar(100) = ''
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT ID
		, FullName
		, Convert(varchar,BirthDate,23) BirthDate
		, TIN
		, EmployeeTypeId
		, isDeleted
	FROM Employee
	WHERE (@ID = 0 OR ID = @ID) AND ISDeleted = 0
	AND (@TIN = '' OR TIN = @TIN)

END
GO


