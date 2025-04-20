const express = require('express');
const cors = require('cors');
const app = express();

app.use(cors()); // Allow frontend to talk to backend
app.use(express.json()); // Parse JSON body

// Example API endpoint
app.get('/api/hello', (req, res) => {
  res.json({ message: 'Hello from API!' });
});

app.listen(3000, () => {
  console.log('API running on http://localhost:3000');
});
