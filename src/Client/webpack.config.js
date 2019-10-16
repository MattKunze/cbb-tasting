var path = require("path")

module.exports = {
  mode: "development",
  entry: "./Client.fsproj",
  output: {
    path: path.join(__dirname, "./public"),
    filename: "bundle.js"
  },
  devServer: {
    contentBase: "./public",
    port: 8080,
    proxy: {
      "/api": {
        target: "http://localhost:" + (process.env.SERVER_PROXY_PORT || "8085"),
        pathRewrite: { "^/api": "" },
        changeOrigin: true
      },
      // redirect websocket requests that start with /socket/* to the server on the port 8085
      "/socket/*": {
        target: "http://localhost:" + (process.env.SERVER_PROXY_PORT || "8085"),
        ws: true
      }
    }
  },
  module: {
    rules: [
      {
        test: /\.fs(x|proj)?$/,
        use: "fable-loader"
      }
    ]
  }
}
