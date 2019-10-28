import * as personOptions from "../actions/personActions";

const initialState = {
  person: [],
  people: [],
  errorMessage: "",
  deleteMsg: ""
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
        people: action.people,
        deleteMsg: ""
      };

    case personOptions.FIND_PERSON:
      return {
        ...state,
        person: action.person
      };

    case personOptions.EDIT_PERSON:
      const index = state.people.findIndex(x => x.id === action.person.id);

      if (index === -1) {
        return {
          ...state,
          errorMessage: "Something went wrong."
        };
      }
      const peopleList = state.people.concat();

      return {
        ...state,
        person: action.person,
        people: peopleList.splice(index, 1, action.person)
      };

    case personOptions.DELETE_PERSON:
      return {
        ...state,
        deleteMsg: action.str,
        people: state.people.filter(x => x.id !== action.id)
      };

    default:
      break;
  }

  return state;
};

export default reducer;
