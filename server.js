const express = require('express');
const cors = require('cors');
const bodyParser = require('body-parser');
const http = require('http')
const path = require('path');
const apiRouter = require(path.join(__dirname, 'api'));

const app = express();
const port = process.env.PORT || 5501;




// Middleware
app.use(cors());
app.use(bodyParser.json());
app.use(express.static('.')); // Serve static files from the current directory

// Use the api router
app.use('/api', apiRouter);


// Create HTTP server
const server = http.createServer(app);

// Start server
server.listen(port, () => {
    console.log(`HTTP Server running at http://localhost:${port}`);
}); 