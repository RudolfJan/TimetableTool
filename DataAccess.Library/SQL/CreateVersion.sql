﻿BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "Version" (
	"Id"	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	"VersionNr"	INTEGER NOT NULL
);

DELETE FROM "Version";
INSERT OR IGNORE INTO "Version" ("VersionNr") VALUES (5);
COMMIT;