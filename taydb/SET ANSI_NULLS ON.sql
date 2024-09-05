SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[add_music]
    @name VARCHAR(200),
    @artist NVARCHAR(255),
    @album NVARCHAR(255),
    @rating DECIMAL(3,2),
    @duration DECIMAL(5,2),
    @status BIT

AS
BEGIN
    SET NOCOUNT ON;

    IF ISNULL(@name, '') = ''
        BEGIN
            SELECT 'INVALID MUSIC NAME' AS [result]
            RETURN
        END

    IF NOT EXISTS (SELECT 1 FROM [dim].[album] WHERE [name] = @album)
        BEGIN
            SELECT 'INVALID ALBUM NAME' AS [result]
            RETURN
        END

    IF NOT EXISTS (SELECT 1 FROM [dim].[album] WHERE [artist] = @artist)
        BEGIN
            SELECT 'INVALID ARTIST' AS [result]
            RETURN
        END

    INSERT INTO 
    [dim].[song]
    ([name], [artist], [duration], [rating], [status], [album])
    VALUES (@name, @artist, @duration, @rating, @status, CAST((SELECT TOP 1 [id] FROM [dim].[album] WHERE [name] = @album) AS INT));

    SELECT 'SUCCESS' AS [result]
END
GO

exec [dbo].[add_music] @name = 'Hot Now', @artist = 'Wiz Khalifa', @album = 'Rolling Papers', @rating = 5, @duration = 10, @status = 1;
select * from [dim].[song]
