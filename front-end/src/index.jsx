import React from "react";
import ReactDOM from "react-dom";
import { Provider } from "react-redux";
import { ConnectedRouter } from "connected-react-router";
import configureStore, { history } from "./configureStore";

import App from "./App";

import "bootstrap";
import "bootstrap/dist/css/bootstrap.css";

import * as serviceWorker from "./serviceWorker";

import StateLoader from "./StateLoader";

import "./Components/css/Style.css";

const stateLoader = new StateLoader();

const store = configureStore(stateLoader.loadState());

store.subscribe(() => {
  stateLoader.saveState(store.getState());
});

// This line is for whenever something goes wrong in the application that requires me to empty the data.
// Tie it together with SignOut later.
// stateLoader.clearState();

ReactDOM.render(
  <Provider store={store}>
    <ConnectedRouter history={history}>
      <App />
    </ConnectedRouter>
  </Provider>,
  document.getElementById("root")
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
