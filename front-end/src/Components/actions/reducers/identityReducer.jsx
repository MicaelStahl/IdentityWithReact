import * as identity from "../actions/identityActions";

const InitialState = {
  user: [],
  users: [],
  isAuthenticated: false,
  message: "",
  email: "",
  roles: []
};

const reducer = (state = InitialState, action) => {
  switch (action.type) {
    case identity.SIGN_IN:
      return {
        ...state,
        isAuthenticated: true,
        message: action.message,
        email: action.email,
        roles: action.roles
      };

    case identity.SIGN_OUT:
      return {
        ...state,
        isAuthenticated: false,
        message: "",
        email: "",
        user: [],
        users: []
      };

    case identity.REGISTER:
      return {
        ...state,
        message: action.message
      };

    default:
      break;
  }
  return state;
};

export default reducer;
