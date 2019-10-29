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

    default:
      break;
  }
  return state;
};

export default reducer;
