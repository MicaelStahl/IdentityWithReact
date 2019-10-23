import React from "react";

import Navbar from "./Components/UI/Navbar";
import Body from "./Components/UI/Body";
import Main from "./Components/UI/Main";
import Footer from "./Components/UI/Footer";

function App() {
  return (
    <React.Fragment>
      <Navbar />
      <Body>
        <Main />
      </Body>
      <Footer />
    </React.Fragment>
  );
}

export default App;
