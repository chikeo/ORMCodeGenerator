CREATE PROC proc_InserttblProducts
(
@createdDate datetime,
@modifiedDate datetime,
@productCode nvarchar(50),
@productDescription nvarchar(50),
@productId numeric,
@productName nvarchar(50)
)
AS
BEGIN
	INSERT INTO tblProducts
	(
	[createdDate],
	[modifiedDate],
	[productCode],
	[productDescription],
	[productId],
	[productName]
	)
VALUES
	(
	@createdDate,
	@modifiedDate,
	@productCode,
	@productDescription,
	@productId,
	@productName
	);
END

CREATE PROC proc_UpdatetblProducts
(
@createdDate datetime,
@modifiedDate datetime,
@productCode nvarchar(50),
@productDescription nvarchar(50),
@productId numeric,
@productName nvarchar(50)
)
AS
BEGIN
	UPDATE tblProducts SET
	[createdDate] = @createdDate,
	[modifiedDate] = @modifiedDate,
	[productCode] = @productCode,
	[productDescription] = @productDescription,
	[productId] = @productId,
	[productName] = @productName
WHERE 
	[productId] = @productId;
END

CREATE PROC proc_DeletetblProducts
(
@productId numeric
)
AS
BEGIN
	DELETE tblProducts
WHERE 
	[productId] = @productId;
END

CREATE PROC proc_GettblProducts
(
@productId numeric
)
AS
BEGIN
	SELECT 
createdDate,
modifiedDate,
productCode,
productDescription,
productId,
productName
FROM tblProducts
WHERE 
	[productId] = @productId;
END
