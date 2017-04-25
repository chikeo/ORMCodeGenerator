CREATE PROC proc_InserttblSupply
(
@createdDate datetime,
@modifiedDate datetime,
@supplyDescription nvarchar(50),
@supplyId numeric,
@supplyName nvarchar(50)
)
AS
BEGIN
	INSERT INTO tblSupply
	(
	[createdDate],
	[modifiedDate],
	[supplyDescription],
	[supplyId],
	[supplyName]
	)
VALUES
	(
	@createdDate,
	@modifiedDate,
	@supplyDescription,
	@supplyId,
	@supplyName
	);
END

CREATE PROC proc_UpdatetblSupply
(
@createdDate datetime,
@modifiedDate datetime,
@supplyDescription nvarchar(50),
@supplyId numeric,
@supplyName nvarchar(50)
)
AS
BEGIN
	UPDATE tblSupply SET
	[createdDate] = @createdDate,
	[modifiedDate] = @modifiedDate,
	[supplyDescription] = @supplyDescription,
	[supplyId] = @supplyId,
	[supplyName] = @supplyName
WHERE 
	[supplyId] = @supplyId;
END

CREATE PROC proc_DeletetblSupply
(
@supplyId numeric
)
AS
BEGIN
	DELETE tblSupply
WHERE 
	[supplyId] = @supplyId;
END

CREATE PROC proc_GettblSupply
(
@supplyId numeric
)
AS
BEGIN
	SELECT 
createdDate,
modifiedDate,
supplyDescription,
supplyId,
supplyName
FROM tblSupply
WHERE 
	[supplyId] = @supplyId;
END
