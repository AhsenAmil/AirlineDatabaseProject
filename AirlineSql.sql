use airline_data;

SET @@time_zone = 'SYSTEM';

CREATE TABLE Passenger(
    passenger_id INTEGER(50) AUTO_INCREMENT,
    first_name VARCHAR(70) NOT NULL DEFAULT 'Unknown',
    last_name VARCHAR(50) NOT NULL DEFAULT 'Unknown',
    email_address NVARCHAR(50) NOT NULL DEFAULT 'Unknown' UNIQUE,
    phone_number INTEGER(50),
    PRIMARY KEY (passenger_id)
);

INSERT INTO Passenger(first_name, last_name, email_address, phone_number) VALUES ('Ava', 'Gardner', 'ava@mail.com', 1234),
                                                                   ('Alan', 'Alda', 'alan@mail.com', 2345),
                                                                   ('Errol', 'Flynn', 'errol@mail.com', 3456),
                                                                   ('Ian', 'Holm', 'ian@mail.com', 4567),
                                                                    ('John', 'Flynn', 'john@mail.com', 5678),
                                                                    ('Lizzie', 'Tomson', 'lizzie@mail.com', 6789);

SHOW COLUMNS FROM Passenger;
SELECT * FROM Passenger;

CREATE TABLE Airplane(
    model_number VARCHAR(50),
    registration_number INTEGER(50) AUTO_INCREMENT,
    capacity INTEGER(255),
    PRIMARY KEY (registration_number)
);
SELECT * FROM Airplane;
INSERT INTO Airplane(model_number, capacity) VALUES ('03MEH', 5);

ALTER TABLE Airplane
ADD COLUMN plane_name VARCHAR(50) NOT NULL DEFAULT 'Unknown';

INSERT INTO Airplane(model_number, capacity, plane_name) VALUES (101, 120, 'Birdy'),
                                                                (202, 110, 'Sebastian'),
                                                                (303, 120, 'Jojo'),
                                                                (404, 120, 'Finn');


SELECT * FROM Airplane;

CREATE TABLE Seat(
  seat_number_p1 INTEGER(10),
  seat_number_p2 VARCHAR(3),
  airplane_reg_number INTEGER(50),
  FOREIGN KEY (airplane_reg_number) REFERENCES Airplane(registration_number),
  PRIMARY KEY (seat_number_p1, seat_number_p2, airplane_reg_number)
);

SELECT * FROM Seat;
DROP TABLE Seat;

DELIMITER $$

CREATE PROCEDURE LoopDemo(IN max_seat INT, IN arn INT)
BEGIN
    DECLARE j INT;

    SET j = 0;

    loop_label: LOOP
        IF j = max_seat THEN
            LEAVE loop_label;
        END IF;

        SET j = j +  1;
        INSERT INTO Seat(seat_number_p1, seat_number_p2,  airplane_reg_number) VALUES (j, 'A', arn),
                                                                                      (j, 'B', arn),
                                                                                      (j, 'C', arn),
                                                                                      (j, 'D', arn),
                                                                                      (j, 'E', arn),
                                                                                      (j, 'F', arn);

    end loop;
end $$

CALL LoopDemo((SELECT @max_seat := capacity FROM Airplane WHERE registration_number=1),1);


CREATE TABLE Flight(
    flight_number INTEGER(255) AUTO_INCREMENT,
    price INTEGER(255),
    destination_airport VARCHAR(255) DEFAULT 'Unknown',
    departure_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    departure_airport VARCHAR(255) DEFAULT 'Unknown',
    arrival_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    flight_type VARCHAR(255),
    airplane_reg_number INTEGER(255),
    gate_number VARCHAR(10),
    PRIMARY KEY (flight_number),
    FOREIGN KEY (airplane_reg_number) REFERENCES Airplane(registration_number)
);
SELECT * FROM Flight;
DROP TABLE Flight;
INSERT INTO Flight(gate_number, price, destination_airport, departure_date, departure_airport, arrival_date, flight_type, airplane_reg_number)
VALUES ('B22', 200, 'LAX', '2020-11-20 10:10:10', 'IST', '2020-11-20 22:10:10', 'International',
        (SELECT registration_number FROM Airplane WHERE registration_number = 1));

INSERT INTO Flight(gate_number, price,  destination_airport, departure_date, departure_airport, arrival_date, flight_type, airplane_reg_number)
VALUES ('B22',400, 'LAX', '2020-11-12 11:10:10', 'IST', '2020-11-12 11:10:10', 'International',
        (SELECT registration_number FROM Airplane WHERE registration_number = 1));

INSERT INTO Flight(gate_number,price, destination_airport, departure_date, departure_airport, arrival_date, flight_type, airplane_reg_number)
VALUES ('A7', 300, 'FRA', '2020-11-21 09:30:50', 'JFK', '2020-11-21 12:30:10', 'International',
        (SELECT registration_number FROM Airplane WHERE registration_number = 2));

INSERT INTO Flight(gate_number, price, destination_airport, departure_date, departure_airport, arrival_date, flight_type, airplane_reg_number)
VALUES ('S09', 245, 'ITA', '2020-11-21 09:30:50', 'LAX', '2020-11-21 12:30:10', 'International',
        (SELECT registration_number FROM Airplane WHERE registration_number = 3));



CREATE TABLE Ticket(
    ticket_number INTEGER(255) AUTO_INCREMENT,
    is_refundable BOOLEAN,
    is_available BOOLEAN,
    seat_number_p1 INTEGER(255),
    seat_number_p2 VARCHAR(255),
    airplane_reg_number INTEGER(255),
    passenger_id INTEGER(255),
    flight_number INTEGER(255),
    PRIMARY KEY (ticket_number),
    FOREIGN KEY (passenger_id) REFERENCES Passenger(passenger_id) ON DELETE CASCADE,
    FOREIGN KEY (flight_number) REFERENCES Flight(flight_number),
    FOREIGN KEY fk_seat_number (seat_number_p1, seat_number_p2, airplane_reg_number) REFERENCES Seat(seat_number_p1, seat_number_p2, airplane_reg_number)
);

INSERT INTO Ticket(is_refundable,is_available, seat_number_p1, seat_number_p2, passenger_id, flight_number, airplane_reg_number)
VALUES (TRUE, FALSE, (SELECT seat_number_p1 FROM Seat WHERE seat_number_p1 = 1 AND seat_number_p2 = 'B' AND Seat.airplane_reg_number = 1),
        (SELECT seat_number_p2 FROM Seat WHERE seat_number_p1 = 1 AND seat_number_p2 = 'B' AND Seat.airplane_reg_number = 1),
        (SELECT passenger_id FROM Passenger WHERE passenger_id = 6),
        (SELECT flight_number FROM Flight WHERE flight_number = 2),
        (SELECT registration_number FROM Airplane WHERE registration_number = 1));

/* JOIN STATEMENTS */
SELECT * FROM Ticket;

DROP TABLE Ticket;
/* passenger names with ticket price 150 */
SELECT first_name, last_name, price
FROM (Passenger JOIN Ticket ON Passenger.passenger_id = Ticket.passenger_id)
WHERE price = 150;

/*  number of tickets for flights grouped by destination */
SELECT destination_airport, COUNT(ticket_number) AS 'Number of Tickets'
FROM (Ticket JOIN Flight ON Ticket.flight_number = Flight.flight_number)
GROUP BY destination_airport;
