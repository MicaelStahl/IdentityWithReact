import * as actionOptions from "../actions/optionsActions";

const initialState = {
  isLoading: false,
  errorMessage: ""
};

const reducer = (state = initialState, action) => {
  switch (action.type) {
    case actionOptions.IS_LOADING:
      return {
        ...state,
        isLoading: action.isLoading,
        errorMessage: ""
      };

    case actionOptions.ERROR_MESSAGE:
      return {
        ...state,
        errorMessage: action.errorMessage
      };

    case actionOptions.SET_USER:
      localStorage.setItem("jwt-token", action.jwtToken);
      localStorage.setItem("active-id", action.activeId);
      break;

    default:
      break;
  }
  return state;
};

export default reducer;
