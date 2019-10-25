import React, { Component } from "react";
import { connect } from "react-redux";

import Title from "../../UI/Title";
import * as options from "../../actions/actions/personActions";

class Edit extends Component {
  state = {
    firstName: null,
    lastName: null,
    age: null,
    email: null,
    phoneNumber: null,
    city: null,
    postalCode: null
  };
  render() {
    return (
      <React.Fragment>
        <Title
          Title={`Editing ${this.props.person.firstName}
            ${this.props.person.lastName}`}
        />
      </React.Fragment>
    );
  }
}

// expand on this more in the future.
const mapStateToProps = state => {
  return {
    person: state.person.person,
    isLoading: state.options.person
  };
};

const mapDispatchToProps = dispatch => {
  return {
    onEditSubmit: person => dispatch(options.EditPersonAsync(person))
  };
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(Edit);
