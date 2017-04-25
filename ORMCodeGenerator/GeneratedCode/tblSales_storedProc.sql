CREATE PROC proc_InserttblSales
(
@actualSellingPrice nvarchar(50),
@costPrice nvarchar(50),
@createdDate datetime,
@modifiedDate datetime,
@packType nvarchar(50),
@productId numeric,
@profit nvarchar(50),
@quantity nvarchar(50),
@saleId numeric,
@supplyId numeric
)
AS
BEGIN
	INSERT INTO tblSales
	(
	[actualSellingPrice],
	[costPrice],
	[createdDate],
	[modifiedDate],
	[packType],
	[productId],
	[profit],
	[quantity],
	[saleId],
	[supplyId]
	)
VALUES
	(
	@actualSellingPrice,
	@costPrice,
	@createdDate,
	@modifiedDate,
	@packType,
	@productId,
	@profit,
	@quantity,
	@saleId,
	@supplyId
	);
END

CREATE PROC proc_UpdatetblSales
(
@actualSellingPrice nvarchar(50),
@costPrice nvarchar(50),
@createdDate datetime,
@modifiedDate datetime,
@packType nvarchar(50),
@productId numeric,
@profit nvarchar(50),
@quantity nvarchar(50),
@saleId numeric,
@supplyId numeric
)
AS
BEGIN
	UPDATE tblSales SET
	[actualSellingPrice] = @actualSellingPrice,
	[costPrice] = @costPrice,
	[createdDate] = @createdDate,
	[modifiedDate] = @modifiedDate,
	[packType] = @packType,
	[productId] = @productId,
	[profit] = @profit,
	[quantity] = @quantity,
	[saleId] = @saleId,
	[supplyId] = @supplyId
WHERE 
	[saleId] = @saleId;
END

CREATE PROC proc_DeletetblSales
(
@saleId numeric
)
AS
BEGIN
	DELETE tblSales
WHERE 
	[saleId] = @saleId;
END

CREATE PROC proc_GettblSales
(
@saleId numeric
)
AS
BEGIN
	SELECT 
actualSellingPrice,
costPrice,
createdDate,
modifiedDate,
packType,
productId,
profit,
quantity,
saleId,
supplyId
FROM tblSales
WHERE 
	[saleId] = @saleId;
END
