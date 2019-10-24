import React from "react";
import Title from "../../UI/Title";

const Delete = props => {
  return (
    <React.Fragment>
      <Title
        Title={`Deleting ${props.person.firstName + props.person.lastName}`}
      />
    </React.Fragment>
  );
};

export default Delete;
