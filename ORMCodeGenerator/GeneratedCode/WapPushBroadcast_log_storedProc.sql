CREATE PROC proc_InsertWapPushBroadcast_log
(
@guid nvarchar(36),
@log_date datetime,
@message_body_out nvarchar(160),
@msisdn nvarchar(13),
@operatorID nvarchar(15),
@shortcode nvarchar(13),
@transaction_id int
)
AS
BEGIN
	INSERT INTO WapPushBroadcast_log
	(
	[guid],
	[log_date],
	[message_body_out],
	[msisdn],
	[operatorID],
	[shortcode],
	[transaction_id]
	)
VALUES
	(
	@guid,
	@log_date,
	@message_body_out,
	@msisdn,
	@operatorID,
	@shortcode,
	@transaction_id
	);
END

CREATE PROC proc_UpdateWapPushBroadcast_log
(
@guid nvarchar(36),
@log_date datetime,
@message_body_out nvarchar(160),
@msisdn nvarchar(13),
@operatorID nvarchar(15),
@shortcode nvarchar(13),
@transaction_id int
)
AS
BEGIN
	UPDATE WapPushBroadcast_log SET
	[guid] = @guid,
	[log_date] = @log_date,
	[message_body_out] = @message_body_out,
	[msisdn] = @msisdn,
	[operatorID] = @operatorID,
	[shortcode] = @shortcode,
	[transaction_id] = @transaction_id
WHERE 
	[guid] = @guid;
END

CREATE PROC proc_DeleteWapPushBroadcast_log
(
@guid nvarchar(36)
)
AS
BEGIN
	DELETE WapPushBroadcast_log
WHERE 
	[guid] = @guid;
END

CREATE PROC proc_GetWapPushBroadcast_log
(
@guid nvarchar(36)
)
AS
BEGIN
	SELECT 
guid,
log_date,
message_body_out,
msisdn,
operatorID,
shortcode,
transaction_id
FROM WapPushBroadcast_log
WHERE 
	[guid] = @guid;
END
