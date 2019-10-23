import React from "react";
import Title from "./Title";

const ErrorPage = props => {
  return (
    <React.Fragment>
      <Title Title="Something went wrong" />
      <div>The given address does not exist. </div>
    </React.Fragment>
  );
};

export default ErrorPage;
