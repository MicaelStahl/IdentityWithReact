import reducers from "./reducers";

// Created in courtesy of https://stackoverflow.com/a/45857898

/**
 * A Statehandler that handles saving, loading, initializing and removal of state-data.
 */
class StateLoader {
  /**
   * Loads a saved store from localStorage.
   * If there exists no saved store, it will return a default store instead.
   */
  loadState() {
    try {
      let serializedState = localStorage.getItem("savedState");

      if (serializedState === null) {
        return this.initializeState();
      }
      return JSON.parse(serializedState);
    } catch (error) {
      throw new Error(error);
    }
    // ToDo
  }
  /**
   * Serializes the given state then saves the given state to localStorage.
   * @param {state} state - The active state
   */
  saveState(state) {
    //ToDo
    try {
      let serializedState = JSON.stringify(state);

      localStorage.setItem("savedState", serializedState);
    } catch (error) {
      throw new Error(error);
    }
  }
  /**
   * Loads a default state if something went wrong.
   */
  initializeState() {
    return reducers;
  }
  /**
   * Clears the entire state in the application.
   */
  clearState() {
    try {
      localStorage.clear();
    } catch (error) {
      throw new Error(error);
    }
  }
}

export default StateLoader;
