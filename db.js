const mysql = require('mysql2/promise');

// Create a connection pool
const pool = mysql.createPool({
    host: 'localhost',
    user: 'Final_Project', // Replace with your MySQL username
    password: 'Password@0312', // Replace with your MySQL password
    database: 'Laptopdb', // Replace with your database name
    waitForConnections: true,
    connectionLimit: 10,
    queueLimit: 0
});

module.exports = pool; 