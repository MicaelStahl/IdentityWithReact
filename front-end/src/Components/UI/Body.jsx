import React, { Fragment } from "react";

const Body = props => {
  return (
    <Fragment>
      <div className="container">{props.children}</div>
    </Fragment>
  );
};

export default Body;
