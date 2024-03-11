const express = require("express");
// var cors = require("cors");
// const qrcode = require('qrcode-terminal');
// const { Client, LocalAuth } = require('whatsapp-web.js');
// const client = new Client({
//   authStrategy: new LocalAuth({
//     dataPath: 'storeSession'
//   })
// });
const app = express();
// app.use(cors());
// app.use(express.urlencoded({ extended: true }));
// app.use(express.json());

// app.use(cookieParser());
// app.use(bodyParser.json());
// app.use(bodyParser.urlencoded({ extended: true }))

// Listening for the server
const PORT = process.env.PORT;
app.listen(PORT, () => console.log(`Server running and listening on port http://localhost:${PORT}`));

app.get('/', async (req, res, next) => {
  // const chats = await client.getChats();
  res.json([chats]);
})

// const country_code = "521";

// app.post('/api/SendWhatsApp', async (req, res, next) => {
//   try
//   {
//     const { Number, Message } = req.body || {}; // Get the body
//     const msg = await client.sendMessage(`${country_code}${Number}@c.us`, Message); // Send the message
//     res.send({ msg }); // Send the response
//   }
//   catch (error)
//   {
//     next(error);
//   }
// })

// const phoneNumber = "5523235261";
// const msg = "Hola Mundo!";

// client.on('qr', (qr) => {
//     console.log('QR RECEIVED', qr);
//     qrcode.generate(qr, { small: true });
//     app.get('/getqr', (req, res, next) => {
//       res.send({ qr });
//     });
// });

// client.on('ready', () => {
//     console.log('Client is ready!');

//     let chatId = country_code + phoneNumber + "@c.us";

//     client.sendMessage(chatId, msg)
//       .then(response => {
//         if(response.id.fromMe)
//           console.log("El mensaje fué enviado");
//       })
// });

// client.on('message', async (message) =>
// {
// 	// const { idSolicitud } = message.body || {}; // Get the body
//   // const res = await fetch(`https://148.204.211.135:8080/api/Solicitudes/filterById/${idSolicitud}`,
//   //   {
//   //     method: 'GET', // *GET, POST, PUT, DELETE, etc.
//   //     mode: 'cors', // no-cors, *cors, same-origin
//   //     cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
//   //     credentials: 'same-origin', // include, *same-origin, omit
//   //     headers:
//   //     {
//   //       'Content-Type': 'application/json' // 'Content-Type': 'application/x-www-form-urlencoded',
//   //     },
//   //     redirect: 'follow', // manual, *follow, error
//   //     referrerPolicy: 'no-referrer', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
//   //     // body: JSON.stringify(message.body) // body data type must match "Content-Type" header
//   //   })
//   //   .then(response => {
//   //     return response.json()
//   //   })
//   //   .then(data => {
//   //     console.log(console.log(data))
//   //   })
//   //   .catch(error => console.error(error));

//   // await message.reply(res.stringify());
//   // await client.sendMessage(message.from, res.json());

//   if (message.body === 'ping')
//   {
// 		await message.reply('pong');
//     await client.sendMessage(message.from, 'pong');

//     let chatId = country_code + phoneNumber + "@c.us";

//     await client.sendMessage(chatId, 'pong')
//       .then(response => {
//         if(response.id.fromMe)
//           console.log("El mensaje fué enviado");
//       })
// 	}
//   else if (message.body === 'chito')
//   {
//     await message.reply('chaz');
//     await client.sendMessage(message.from, 'chaztin');
//   }
// });

// client.initialize();