const express = require("express");
var cors = require("cors");
var app = express();
app.use(cors());
app.use(express.urlencoded({ extended: true }));
app.use(express.json());
// app.use(cookieParser());
// app.use(bodyParser.json());
// app.use(bodyParser.urlencoded({ extended: true }))

// Listening for the server
const PORT = process.env.PORT || 8081;
app.listen(PORT, () => console.log(`Server running and listening on port http://148.204.211.135:${PORT}`));

app.get('/', async (req, res, next) => {
  console.log("Hola Mundo");
  res.json([chats]);
})
