BEGIN TRANSACTION;
DELETE FROM "ServiceClasses";
INSERT INTO "ServiceClasses" ("Id","ServiceClassName","ServiceClassDescription","Category","Color") VALUES 
 (1,'Passenger','Unspecified passenger services','Passenger','Blue'),
 (2,'Freight','Unspecified Freight services','Freight','Red'),
 (3,'Stopping passenger','Stopping passenger service, stops at most stations','Passenger','RoyalBlue'),
 (4,'Regio express passenger','Passenger, limited stops at major stations','Passenger','MediumBlue'),
 (5,'Express passenger','Passenger, stops only at main stations','Passenger','SlateBlue'),
 (6,'International passenger','International passenger trains','Passenger','Indigo'),
 (7,'Passenger depot run','Passenger trains for depot','Passenger','Green'),
 (8,'Light engine','Light engine','Other','Olive'),
 (9,'Freight shunting duty','Freight Shunting','Freight','Orange'),
 (10,'Heavy freight','Heavy freight','Freight','OrangeRed'),
 (11,'Express freight','Express freight','Freight','Tomato'),
 (12,'Work train','Work train, track maintenance etcetera','Other','Lime');
 (13,'AI Freight','Freight, AI only','Freight','Pink'),
 (14,'AI Passenger','Passenger, AI only','Passenger','SkyBlue');
COMMIT;


