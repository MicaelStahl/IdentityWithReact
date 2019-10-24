import React from "react";
import { withRouter } from "react-router-dom";

/**
 * A function that creates a "go back" button that is used across the application.
 * Especially used for admin with Creating/reading/updating/deleting products.
 */
const GoBack = props => {
  const { float } = props;
  return (
    <button
      onClick={() => onClick(props)}
      className={`btn btn-primary btn-sm mb-3 ${
        float === true ? "float-left" : null
      }`}>
      Return
    </button>
  );
};

const onClick = props => {
  props.history.goBack();
  // props.onBtnClick();
};

export default withRouter(GoBack);
