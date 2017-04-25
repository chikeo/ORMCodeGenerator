CREATE PROC proc_InserttblProductSupply
(
@costPrice nvarchar(50),
@createdDate datetime,
@maxSellingPrice nvarchar(50),
@minSellingPrice nvarchar(50),
@modifiedDate datetime,
@productId numeric,
@productSupplyId numeric,
@quantityAtHand nvarchar(50),
@suppliedQuantity nvarchar(50),
@supplyId numeric
)
AS
BEGIN
	INSERT INTO tblProductSupply
	(
	[costPrice],
	[createdDate],
	[maxSellingPrice],
	[minSellingPrice],
	[modifiedDate],
	[productId],
	[productSupplyId],
	[quantityAtHand],
	[suppliedQuantity],
	[supplyId]
	)
VALUES
	(
	@costPrice,
	@createdDate,
	@maxSellingPrice,
	@minSellingPrice,
	@modifiedDate,
	@productId,
	@productSupplyId,
	@quantityAtHand,
	@suppliedQuantity,
	@supplyId
	);
END

CREATE PROC proc_UpdatetblProductSupply
(
@costPrice nvarchar(50),
@createdDate datetime,
@maxSellingPrice nvarchar(50),
@minSellingPrice nvarchar(50),
@modifiedDate datetime,
@productId numeric,
@productSupplyId numeric,
@quantityAtHand nvarchar(50),
@suppliedQuantity nvarchar(50),
@supplyId numeric
)
AS
BEGIN
	UPDATE tblProductSupply SET
	[costPrice] = @costPrice,
	[createdDate] = @createdDate,
	[maxSellingPrice] = @maxSellingPrice,
	[minSellingPrice] = @minSellingPrice,
	[modifiedDate] = @modifiedDate,
	[productId] = @productId,
	[productSupplyId] = @productSupplyId,
	[quantityAtHand] = @quantityAtHand,
	[suppliedQuantity] = @suppliedQuantity,
	[supplyId] = @supplyId
WHERE 
	[productSupplyId] = @productSupplyId;
END

CREATE PROC proc_DeletetblProductSupply
(
@productSupplyId numeric
)
AS
BEGIN
	DELETE tblProductSupply
WHERE 
	[productSupplyId] = @productSupplyId;
END

CREATE PROC proc_GettblProductSupply
(
@productSupplyId numeric
)
AS
BEGIN
	SELECT 
costPrice,
createdDate,
maxSellingPrice,
minSellingPrice,
modifiedDate,
productId,
productSupplyId,
quantityAtHand,
suppliedQuantity,
supplyId
FROM tblProductSupply
WHERE 
	[productSupplyId] = @productSupplyId;
END
