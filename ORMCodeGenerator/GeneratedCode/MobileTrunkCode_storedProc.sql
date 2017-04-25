CREATE PROC proc_InsertMobileTrunkCode
(
@id int,
@operatorID nvarchar(15),
@trunkCode nvarchar(5)
)
AS
BEGIN
	INSERT INTO MobileTrunkCode
	(
	[id],
	[operatorID],
	[trunkCode]
	)
VALUES
	(
	@id,
	@operatorID,
	@trunkCode
	);
END

CREATE PROC proc_UpdateMobileTrunkCode
(
@id int,
@operatorID nvarchar(15),
@trunkCode nvarchar(5)
)
AS
BEGIN
	UPDATE MobileTrunkCode SET
	[id] = @id,
	[operatorID] = @operatorID,
	[trunkCode] = @trunkCode
WHERE 
	[id] = @id;
END

CREATE PROC proc_DeleteMobileTrunkCode
(
@id int
)
AS
BEGIN
	DELETE MobileTrunkCode
WHERE 
	[id] = @id;
END

CREATE PROC proc_GetMobileTrunkCode
(
@id int
)
AS
BEGIN
	SELECT 
id,
operatorID,
trunkCode
FROM MobileTrunkCode
WHERE 
	[id] = @id;
END
