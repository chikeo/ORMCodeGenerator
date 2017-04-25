CREATE PROC proc_InserttblExpenses
(
@createdDate datetime,
@expenseAmount nvarchar(50),
@expenseDescription nvarchar(50),
@expenseId numeric,
@expenseName nvarchar(50),
@modifiedDate datetime
)
AS
BEGIN
	INSERT INTO tblExpenses
	(
	[createdDate],
	[expenseAmount],
	[expenseDescription],
	[expenseId],
	[expenseName],
	[modifiedDate]
	)
VALUES
	(
	@createdDate,
	@expenseAmount,
	@expenseDescription,
	@expenseId,
	@expenseName,
	@modifiedDate
	);
END

CREATE PROC proc_UpdatetblExpenses
(
@createdDate datetime,
@expenseAmount nvarchar(50),
@expenseDescription nvarchar(50),
@expenseId numeric,
@expenseName nvarchar(50),
@modifiedDate datetime
)
AS
BEGIN
	UPDATE tblExpenses SET
	[createdDate] = @createdDate,
	[expenseAmount] = @expenseAmount,
	[expenseDescription] = @expenseDescription,
	[expenseId] = @expenseId,
	[expenseName] = @expenseName,
	[modifiedDate] = @modifiedDate
WHERE 
	[expenseId] = @expenseId;
END

CREATE PROC proc_DeletetblExpenses
(
@expenseId numeric
)
AS
BEGIN
	DELETE tblExpenses
WHERE 
	[expenseId] = @expenseId;
END

CREATE PROC proc_GettblExpenses
(
@expenseId numeric
)
AS
BEGIN
	SELECT 
createdDate,
expenseAmount,
expenseDescription,
expenseId,
expenseName,
modifiedDate
FROM tblExpenses
WHERE 
	[expenseId] = @expenseId;
END
