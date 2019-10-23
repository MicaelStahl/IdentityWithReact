import * as actionOptions from "../actions/optionsActions";

const initialState = {
  isLoading: false
};

const reducer = (state = initialState, action) => {
  switch (action.type) {
    case actionOptions.IS_LOADING:
      return {
        ...state,
        isLoading: action.isLoading
      };

    default:
      break;
  }
  return state;
};

export default reducer;
