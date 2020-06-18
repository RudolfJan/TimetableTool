BEGIN TRANSACTION;
INSERT INTO "ServiceClasses" ("Id","ServiceClassName","ServiceClassDescription","Category") VALUES (1,'Passenger','Unspecified passenger services','Passenger'),
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
COMMIT;
