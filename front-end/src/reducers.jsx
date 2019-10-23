import { combineReducers } from "redux";
import { connectRouter } from "connected-react-router";

import personReducer from "./Components/actions/reducers/personReducer";
import optionsReducer from "./Components/actions/reducers/optionsReducer";

export default history =>
  combineReducers({
    router: connectRouter(history),
    person: personReducer,
    options: optionsReducer
  });
