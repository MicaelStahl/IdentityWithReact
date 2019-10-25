import * as options from "./optionsActions";
import axios from "axios";
import { isNullOrUndefined } from "util";

export const CREATE_PERSON = "CREATE_PERSON";
export const FIND_PERSON = "FIND_PERSON";
export const FIND_PEOPLE = "FIND_PEOPLE";
export const EDIT_PERSON = "EDIT_PERSON";
export const DELETE_PERSON = "DELETE_PERSON";

const url = "http://localhost:49420/api/PersonApi/";

//#region Create

const CreatePerson = person => {
  return {
    type: CREATE_PERSON,
    person
  };
};

/**
 * Method used for Person creation.
 * @param {person} person - Variable to create.
 */
export const CreatePersonAsync = person => {
  return dispatch => {
    axios
      .post(url + "Create", person, {
        cancelToken: options.CancelToken(),
        validateStatus: function(status) {
          return status <= 500;
        }
      })
      .then(response => {
        if (response.status === 200) {
          dispatch(CreatePerson(response.data));
        } else {
          // ToDo
        }
      })
      .catch(err => {
        console.error(err);
      });
  };
};

//#endregion

//#region Find

//#region FindOne

/**
 * Function used for saving one person to the redux store.
 * @param {person} person The person to save to the redux store
 */
export const FindPerson = person => {
  return {
    type: FIND_PERSON,
    person
  };
};

/**
 * Finds one specific user in provided list if one was included.
 * Else it calls back-end for a person with the specific id
 * @param {id} id The id for the requested person
 * @param {people} people The list of people in the application - Optional
 */
export const FindPersonAsync = (id, people = []) => {
  return dispatch => {
    // If a list was included it will go through it to find the requested person.
    if (!isNullOrUndefined(people) || people.length !== 0) {
      const person = people.find(x => x.id === id);

      // If the person was found it will call another method that saves the person to the redux store.
      // Else it will call the backend for the user.
      if (person !== undefined) {
        dispatch(FindPerson(person));

        return;
      }
    }

    dispatch(options.IsLoading(true));

    axios
      .get(url + "Find" + id, {
        "Content-Type": "application/json",
        cancelToken: options.CancelToken(),
        validateStatus: function(status) {
          return status <= 500;
        }
      })
      .then(response => {
        if (response.status === 200) {
          dispatch(FindPerson(response.data));
        } else {
          // ToDo
        }
        dispatch(options.IsLoading(false));
      })
      .catch(err => {
        console.error(err);
      });
  };
};

//#endregion

//#region FindAll

const FindAll = people => {
  return {
    type: FIND_PEOPLE,
    people
  };
};

export const FindAllAsync = () => {
  return dispatch => {
    dispatch(options.IsLoading(true));
    axios
      .get(url + "find-all", {
        "Content-Type": "application/json",
        cancelToken: options.CancelToken(),
        validateStatus: function(status) {
          return status <= 500;
        }
      })
      .then(response => {
        if (response.status === 200) {
          dispatch(FindAll(response.data.people));
        } else {
          // ToDo
        }
        dispatch(options.IsLoading(false));
      })
      .catch(err => {
        console.error(err);
      });
  };
};

//#endregion

//#endregion

//#region Edit

const EditPerson = person => {
  return {
    type: EDIT_PERSON,
    person
  };
};

export const EditPersonAsync = person => {
  return dispatch => {
    axios
      .put(url + "edit", person, {
        cancelToken: options.CancelToken(),
        validateStatus: function(status) {
          return status <= 500;
        }
      })
      .then(response => {
        if (response.status === 200) {
          dispatch(EditPerson(response.data));
        } else {
          // ToDo
        }
      })
      .catch(err => {
        console.error(err);
      });
  };
};

//#endregion

//#region Delete

const DeletePerson = (str, id) => {
  return {
    type: DELETE_PERSON,
    str,
    id
  };
};

export const DeletePersonAsync = id => {
  return dispatch => {
    axios
      .delete(url + "delete/" + id, {
        "Content-Type": "application/json",
        cancelToken: options.CancelToken(),
        validateStatus: function(status) {
          return status <= 500;
        }
      })
      .then(response => {
        if (response.status === 200) {
          dispatch(DeletePerson(response.data, id));
        } else {
          // ToDo
        }
      })
      .catch(err => {
        console.error(err);
      });
  };
};

//#endregion
