BEGIN TRANSACTION;

CREATE TABLE IF NOT EXISTS "ServiceClasses" (
	"Id"	INTEGER NOT NULL,
	"ServiceClassName"	TEXT NOT NULL,
	"ServiceClassDescription"	TEXT NOT NULL,
	Category TEXT NOT NULL,
	UNIQUE("ServiceClassName"),
	PRIMARY KEY("Id")
	);

CREATE TABLE IF NOT EXISTS "TimeEventTypes" (
	"Id"	INTEGER NOT NULL,
	"EventType"	TEXT NOT NULL,
	"EventTypeDescription"	TEXT NOT NULL DEFAULT '',
	UNIQUE("EventType")
	PRIMARY KEY("Id")
);

-- Insert data into configuration tables
INSERT OR IGNORE INTO "TimeEventTypes" ("Id","EventType","EventTypeDescription") VALUES 
 (1,'S','Start service'),
 (2,'E','End service'),
 (3,'P','Pass through location'),
 (4,'H','Halt, pick up passengers'),
 (5,'W','Wait, Intermediate stop'),
 (6,'SE','Shunt activities at single location'),
 (7,'--','No action, just show the location');

 INSERT INTO "ServiceClasses" ("Id","ServiceClassName","ServiceClassDescription","Category") VALUES 
 (1,'Passenger','Unspecified passenger services','Passenger'),
 (2,'Freight','Unspecified Freight services','Freight'),
 (3,'Stopping passenger','Stopping passenger service, stops at most stations','Passenger'),
 (4,'Regio express passenger','Passenger, limited stops at major stations','Passenger'),
 (5,'Express passenger','Passenger, stops only at main stations','Passenger'),
 (6,'International passenger','International passenger trains','Passenger'),
 (7,'Passenger depot run','Passenger trains for depot','Passenger'),
 (8,'Light engine','Light engine','Other'),
 (9,'Freight shunting duty','Freight Shuntiing','Freight'),
 (10,'Heavy freight','Heavy freight','Freight'),
 (11,'Express freight','Express freight','Freight'),
 (12,'Work train','Work train, track maintenance etcetera','Other');

DELETE FROM "Version";
INSERT OR IGNORE INTO "Version" ("VersionNr") VALUES (4);
COMMIT;
