CREATE TABLE apple_podcasts
(
   id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
   collection_id NVARCHAR(32) NOT NULL,
   collection_name NVARCHAR(512) NOT NULL,
   artist_name NVARCHAR(256),
   feed_url NVARCHAR(1024),
   release_date DATETIME,
   track_count INT,
   country NVARCHAR(64)
)

