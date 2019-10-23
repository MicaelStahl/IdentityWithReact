import * as personOptions from "../actions/personActions";

const initialState = {
  person: [],
  people: []
};

const reducer = (state = initialState, action) => {
  console.log("Action", action);
  switch (action.type) {
    case personOptions.CREATE_PERSON:
      const list = state.people.concat(action.person);
      return {
        ...state,
        person: action.person,
        people: list
      };

    case personOptions.FIND_PEOPLE:
      return {
        ...state,
        people: action.people
      };

    default:
      break;
  }

  return state;
};

export default reducer;
