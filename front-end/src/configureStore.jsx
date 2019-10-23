import { createBrowserHistory } from "history";
import { applyMiddleware, compose, createStore } from "redux";
import { routerMiddleware } from "connected-react-router";
import CreateRootReducer from "./reducers";
import thunk from "redux-thunk";

const baseUrl = document.getElementsByTagName("base")[0].getAttribute("href");
export const history = createBrowserHistory({ basename: baseUrl });

/**
 * Configures the store to be used in Index.jsx.
 * @param {preloadedState} preloadedState - The loaded state from StateLoader.
 */
export default function configureStore(preloadedState) {
  const store = createStore(
    CreateRootReducer(history),
    preloadedState,
    compose(applyMiddleware(routerMiddleware(history), thunk))
  );

  return store;
}
