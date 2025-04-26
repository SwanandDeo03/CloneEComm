const mysql = require('mysql');


// Create a connection pool
const pool = mysql.createPool({ 
    host: 'localhost', // Do not change this host if you are using local server
    user: 'Final_Project', // Do not change the user
    password: 'Password@0312', // Do not change the password
    database: 'Laptopdb', // Do not change the database name
    waitForConnections: true, 
    connectionLimit: 10, 
    queueLimit: 0 
});

module.exports = pool;