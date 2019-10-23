import React from "react";

/**
 *
 * @param {Title} Title - The title shown.
 * @param {isLoading} isLoading - Optional - Indicates whether the application is loading a Async call.
 */
const Title = props => {
  const { Title, isLoading } = props;
  return (
    <React.Fragment>
      <h1 className="text-left container">
        {isLoading === true ? (
          <i className="fa fa-spinner fa-spin mr-1" />
        ) : null}
        {Title}
      </h1>

      <hr />
    </React.Fragment>
  );
};

export default Title;
