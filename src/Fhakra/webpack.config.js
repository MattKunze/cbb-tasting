var path = require("path")

module.exports = {
  mode: "development",
  entry: "./Fhakra.fsproj",
  output: {
    path: path.join(__dirname, "./public"),
    filename: "bundle.js"
  },
  devServer: {
    contentBase: "./public",
    port: 8081
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
